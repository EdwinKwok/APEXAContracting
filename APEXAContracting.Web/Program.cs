using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APEXAContracting.Common.Logging;

namespace APEXAContracting.Web
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
                    webBuilder.UseStartup<Startup>();
                }).ConfigureLogging(c =>
                {
                    c.AddFile(); // Use the custom FileLoggerProvider. Save log into Logs folder.
                }).ConfigureAppConfiguration((hostingContext, config) =>
                {
                    //
                    // Load different appsetting.json settings based on currnt development environment setup.
                    // Corporate with different developers local development.
                    //
                    var env = hostingContext.HostingEnvironment;
                    config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                    .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
                    .AddJsonFile($"appsettings.{System.Environment.MachineName}.json", optional: true, reloadOnChange: true)
                    .AddJsonFile($"appsettings.{System.Environment.UserName}.json", optional: true, reloadOnChange: true);
                    config.AddEnvironmentVariables();
                });
    }
}
