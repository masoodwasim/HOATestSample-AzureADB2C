using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HOATest.API.DataContext;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace HOATest.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateWebHostBuilder(args).Build();
            //2. Find the service layer within our scope.
            using (var scope = host.Services.CreateScope())
            {
                //3. Get the instance of HOATestDBContext in our services layer
                var services = scope.ServiceProvider;
                var context = services.GetRequiredService<HOATestDBContext>();

                //4. Call the BooksDataSeed to create sample data
                BooksDataSeed.Initialize(services);
            }
            host.Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
           WebHost.CreateDefaultBuilder(args)
               .UseStartup<Startup>();
    }
}
