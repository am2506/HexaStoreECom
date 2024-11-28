using HexaStoreECom.DataAccess;
using HexaStoreECom.DataAccess.DataSeed;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HexaStoreECom.Web
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
           var host= CreateHostBuilder(args).Build();

            using (var scopped = host.Services.CreateScope())
            {
                var serviceProvider = scopped.ServiceProvider;
                try
                {
                    var _dbContext = serviceProvider.GetRequiredService<ApplicationDbContext>();
                    await DataSeedingContext.CategoryDataSeed(_dbContext);
                    await DataSeedingContext.ProductsDataSeed(_dbContext);

                }

                catch
                {

                }
            }

            host.Run();

        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
