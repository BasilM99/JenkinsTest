using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ArabyAds.AdFalcon.API.Core
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Directory.SetCurrentDirectory(AppContext.BaseDirectory);
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseWindowsService().ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseKestrel((builderContext, options) =>
                    {
                        var kestrelSection = builderContext.Configuration.GetSection("Kestrel");
                        options.Configure(kestrelSection);
                        var kestrelOptions = kestrelSection.Get<KestrelServerOptions>();
                        if (kestrelOptions != null)
                        {
                            options.AllowSynchronousIO = kestrelOptions.AllowSynchronousIO;
                           
                        }
                    })
                    .UseContentRoot(AppContext.BaseDirectory)
                    .UseStartup<Startup>();
                });
    }
}
