using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace HelloWorld
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.ConfigureAppConfiguration((context, config) =>
                    {
                        SetupAzureAppConfiguration(config);
                    });
                    webBuilder.ConfigureKestrel(options => options.AddServerHeader = false);
                    webBuilder.UseStartup<Startup>().CaptureStartupErrors(true); ;
                });

        /// <summary>
        /// Sets up Azure App Configuration adn feature flags.
        /// See https://github.com/Azure/AppConfiguration
        /// See https://github.com/microsoft/FeatureManagement-Dotnet.
        /// </summary>
        /// <param name="config">The configuration builder.</param>
        private static void SetupAzureAppConfiguration(IConfigurationBuilder config)
        {
            if (config is null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            var configuration = config.Build();
            config.AddAzureAppConfiguration(options =>
            {
                var connectionString = $"Endpoint={configuration["AppConfig:ConnectionString"]}";
                options.Connect(connectionString)
                        .UseFeatureFlags((op) =>
                        {
                            _ = int.TryParse(configuration["AppConfig:CacheExpirationTimeInSeconds"], out var timeOut);
                            op.CacheExpirationInterval = TimeSpan.FromSeconds(timeOut);
                        });
            });
        }
    }
}
