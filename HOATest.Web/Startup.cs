using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Threading;
using System.Threading.Tasks;
using HOATest.Web.Infrastructure;
using HOATest.Web.Proxy;
using HOATest.Web.AzureAD;
using AutoMapper;

namespace HOATest.Web
{
    public class Startup
    {
        private readonly IConfiguration configuration;
        public Startup(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddAuthentication(AzureADB2CDefaults.AuthenticationScheme)
            //    .AddAzureADB2C(options => Configuration.Bind("AzureAdB2C", options));

            IdentityModelEventSource.ShowPII = true;
            services.Configure<B2CAuthenticationOptions>(configuration.GetSection("Authentication:AzureAd"));
            services.Configure<B2CPolicies>(configuration.GetSection("Authentication:AzureAd:B2C"));

            services.Configure<TestServiceOptions>(configuration.GetSection("TestServiceOptions"));
            services.AddTransient<TestServiceProxy>();

            services.AddControllersWithViews(options => options.Filters.Add(typeof(ReauthenticationRequiredFilter)));

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddDistributedMemoryCache();

            ConfigureAuthentication(services);
            services.AddAutoMapper(typeof(Startup));
            services.AddControllersWithViews();
            services.AddRazorPages();
        }
        private static void ConfigureAuthentication(IServiceCollection services)
        {
            var serviceProvider = services.BuildServiceProvider();

            var authOptions = serviceProvider.GetService<IOptions<B2CAuthenticationOptions>>();
            var b2cPolicies = serviceProvider.GetService<IOptions<B2CPolicies>>();

            var distributedCache = serviceProvider.GetService<IDistributedCache>();
            services.AddSingleton(distributedCache);

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = Constants.OpenIdConnectAuthenticationScheme;
            })
            .AddCookie()
            .AddOpenIdConnect(Constants.OpenIdConnectAuthenticationScheme, options =>
            {
                options.Authority = authOptions.Value.Authority;
                options.ClientId = authOptions.Value.ClientId;
                options.ClientSecret = authOptions.Value.ClientSecret;
                options.SignedOutRedirectUri = authOptions.Value.PostLogoutRedirectUri;

                options.ConfigurationManager = new PolicyConfigurationManager(authOptions.Value.Authority,
                                               new[] { b2cPolicies.Value.SignInOrSignUpPolicy, b2cPolicies.Value.EditProfilePolicy, b2cPolicies.Value.ResetPasswordPolicy });

                options.Events = CreateOpenIdConnectEventHandlers(authOptions.Value, b2cPolicies.Value, distributedCache);

                options.ResponseType = OpenIdConnectResponseType.CodeIdToken;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    NameClaimType = "name"
                };

                // it will fall back on using DefaultSignInScheme if not set
                //options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;

                // we have to set these scope that will be used in /authorize request
                // (otherwise the /token request will not return access and refresh tokens)
                options.Scope.Add("offline_access");
                options.Scope.Add($"{authOptions.Value.ApiIdentifier}/read_values");

                // this can be used if the middleware redeems the authorization code
                //options.SaveTokens = true;
            });
        }
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }

        private static OpenIdConnectEvents CreateOpenIdConnectEventHandlers(B2CAuthenticationOptions authOptions, B2CPolicies policies, IDistributedCache distributedCache)
        {
            return new OpenIdConnectEvents
            {
                OnRedirectToIdentityProvider = context => SetIssuerAddressAsync(context, policies.SignInOrSignUpPolicy),
                OnRedirectToIdentityProviderForSignOut = context => SetIssuerAddressForSignOutAsync(context, policies.SignInOrSignUpPolicy),
                OnAuthorizationCodeReceived = async context =>
                {
                    try
                    {
                        var principal = context.Principal;

                        var userTokenCache = new DistributedTokenCache(distributedCache, principal.FindFirst(Constants.ObjectIdClaimType).Value).GetMSALCache();
                        var client = new ConfidentialClientApplication(authOptions.ClientId,
                            authOptions.GetAuthority(principal.FindFirst(Constants.AcrClaimType).Value),
                            "https://app", // it's not really needed
                            new ClientCredential(authOptions.ClientSecret),
                            userTokenCache,
                            null);

                        var result = await client.AcquireTokenByAuthorizationCodeAsync(context.TokenEndpointRequest.Code,
                            new[] { $"{authOptions.ApiIdentifier}/read_values" });

                        context.HandleCodeRedemption(result.AccessToken, result.IdToken);
                    }
                    catch (Exception ex)
                    {
                        context.Fail(ex);
                    }
                },
                OnAuthenticationFailed = context =>
                {
                    context.Fail(context.Exception);
                    return Task.FromResult(0);
                },
                OnMessageReceived = context =>
                {
                    if (!string.IsNullOrEmpty(context.ProtocolMessage.Error) &&
                        !string.IsNullOrEmpty(context.ProtocolMessage.ErrorDescription))
                    {
                        if (context.ProtocolMessage.ErrorDescription.StartsWith("AADB2C90091")) // cancel profile editing
                        {
                            context.HandleResponse();
                            context.Response.Redirect("/");
                        }
                        else if (context.ProtocolMessage.ErrorDescription.StartsWith("AADB2C90118")) // forgot password
                        {
                            context.HandleResponse();
                            context.Response.Redirect("/Account/ResetPassword");
                        }
                    }

                    return Task.FromResult(0);
                }
            };
        }

        private static async Task SetIssuerAddressAsync(RedirectContext context, string defaultPolicy)
        {
            var configuration = await GetOpenIdConnectConfigurationAsync(context, defaultPolicy);
            context.ProtocolMessage.IssuerAddress = configuration.AuthorizationEndpoint;
        }

        private static async Task SetIssuerAddressForSignOutAsync(RedirectContext context, string defaultPolicy)
        {
            var configuration = await GetOpenIdConnectConfigurationAsync(context, defaultPolicy);
            context.ProtocolMessage.IssuerAddress = configuration.EndSessionEndpoint;
        }

        private static Task<OpenIdConnectConfiguration> GetOpenIdConnectConfigurationAsync(RedirectContext context, string defaultPolicy)
        {
            var manager = (PolicyConfigurationManager)context.Options.ConfigurationManager;
            var policy = context.Properties.Items.ContainsKey(Constants.B2CPolicy) ? context.Properties.Items[Constants.B2CPolicy] : defaultPolicy;

            return manager.GetConfigurationByPolicyAsync(CancellationToken.None, policy);
        }
    }
}
