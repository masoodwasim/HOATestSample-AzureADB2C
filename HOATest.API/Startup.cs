using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using HOATest.API.AzureAD;
using Microsoft.IdentityModel.Logging;
using HOATest.API.DataContext;
using HOATest.API.Business;
using Microsoft.EntityFrameworkCore;


namespace HOATest.API
{
    public class Startup
    {
        private readonly IConfiguration configuration;
        public Startup(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            IdentityModelEventSource.ShowPII = true;
            services.AddDbContext<HOATestDBContext>(options => options.UseInMemoryDatabase(databaseName: "HOATestDB"));
            services.AddTransient<IBooksManager, BooksManager>();
            var authOptions = configuration.GetSection("Authentication:AzureAd").Get<AuthenticationOptions>();
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme) // sets both authenticate and challenge default schemes
                .AddJwtBearer(options =>
                {
                    options.MetadataAddress = $"{authOptions.Authority}/.well-known/openid-configuration?p={authOptions.SignInOrSignUpPolicy}";
                    options.Audience = authOptions.Audience;

                });

            services.AddAuthorization(options =>
                options.AddPolicy("ReadValuesPolicy", config => config.RequireClaim("http://schemas.microsoft.com/identity/claims/scope", new[] { "read_values" })));

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1",
                new OpenApiInfo
                {
                    Title = "HOA Test API",
                    Version = "v1",
                    Description = "API's to fetch current best Book titles around the world!",
                    TermsOfService = new Uri("http://tempuri.org/terms"),
                    Contact = new OpenApiContact
                    {
                        Name = "WasimUl Masood",
                        Email = "masoodwasim@outlook.com"
                    }
                }
            );
            });

            services.AddSwaggerGen(document =>
            {
                document.AddSecurityDefinition("bearer", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.OAuth2,
                    Description = "B2C authentication",
                    Flows = new OpenApiOAuthFlows()
                    {
                        Implicit = new OpenApiOAuthFlow()
                        {
                            Scopes = new Dictionary<string, string>
                        {
                            { "https://datasquaredev.onmicrosoft.com/testapi/user_impersonation", "Access the api as the signed-in user" },
                           // { "https://datasquaredev.onmicrosoft.com/testapi/read_values", "Read access to the API"},
                           
                        },
                            AuthorizationUrl = new Uri("https://datasquaredev.b2clogin.com/datasquaredev.onmicrosoft.com/oauth2/v2.0/authorize?p=b2c_1_testsignupandsigninpolicy", UriKind.Absolute),
                            TokenUrl = new Uri("https://datasquaredev.b2clogin.com/datasquaredev.onmicrosoft.com/oauth2/v2.0/token?p=b2c_1_testsignupandsigninpolicy", UriKind.Absolute)
                        },
                    }
                });
            });

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "HOA Test API");
                c.OAuthClientId("f75455ae-2b1b-4e8f-81af-dacfb82f9bde");
            });


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

        }
    }
}
