using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Sixpence.Web.Config;

namespace Sixpence.Web.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            CreateHostBuilder(new string[] { })
                .Build()
                .Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder
                    .UseUrls(SystemConfig.Config.LocalUrls)
                    .UseStartup<Startup>();
            });
    }
}

