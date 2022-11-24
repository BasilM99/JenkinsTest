using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using ArabyAds.AdFalcon.Services.Mapping;
using ArabyAds.Framework.Utilities;

namespace ArabyAds.AdFalcon.Services
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Directory.SetCurrentDirectory(AppContext.BaseDirectory);
            CreateHostBuilder(args).Build().Run();
        
        }

        // Additional configuration is required to successfully run gRPC on macOS.
        // For instructions on how to configure Kestrel and gRPC clients on macOS, visit https://go.microsoft.com/fwlink/?linkid=2099682
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder(args)
            .UseWindowsService()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseKestrel((builderContext, options) =>
                    {
                        var kestrelSection = builderContext.Configuration.GetSection("Kestrel");
                        options.Configure(kestrelSection);
                        var kestrelOptions = kestrelSection.Get<KestrelServerOptions>();
                        if (kestrelOptions != null)
                        {
                            options.AllowSynchronousIO = kestrelOptions.AllowSynchronousIO;
                            options.Limits.MaxRequestBodySize = kestrelOptions.Limits.MaxRequestBodySize;
                            options.Limits.MaxRequestBufferSize = kestrelOptions.Limits.MaxRequestBufferSize;
                            options.Limits.MaxResponseBufferSize = kestrelOptions.Limits.MaxResponseBufferSize;
                        }
                    })
                    .UseContentRoot(AppContext.BaseDirectory)
                    .UseStartup<Startup>();
                    //var value1= ArabyAds.Framework.Utilities.Cryptography.protect("Anas");
                   // var value2 =  ArabyAds.Framework.Utilities.Cryptography.Unprotect(value1);
                });
    }
}
