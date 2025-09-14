using Azure.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;
namespace TodoApi
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
                    // Add Azure App Configuration here, before Startup
                    webBuilder.ConfigureAppConfiguration((hostingContext, config) =>
                    {
                        var settings = config.Build();
                        var endpoint = settings["AppConfig:Endpoint"];

                        if (!string.IsNullOrEmpty(endpoint))
                        {
                            // Connect to Azure App Configuration using Managed Identity
                            config.AddAzureAppConfiguration(options =>
                                options.Connect(
                                    new Uri(endpoint),
                                    new ManagedIdentityCredential()));
                        }
                    });

                    // This remains the same
                    webBuilder.UseStartup<Startup>();
                });
    }
}