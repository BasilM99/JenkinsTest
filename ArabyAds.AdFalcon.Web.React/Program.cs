using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace ArabyAds.AdFalcon.Web.React
{
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                var builder = CreateHostBuilder(args);

                if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                    builder.UseSystemd();
                else
                    builder.UseWindowsService();

                builder.Build().Run();
            }
            catch (Exception ex)
            {
                //log.Fatal("", ex);
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseKestrel((builderContext, options) =>
                    {
                        var kestrelSection = builderContext.Configuration.GetSection("Kestrel");
                        options.Configure(kestrelSection);
                        var kestrelOptions = kestrelSection.Get<KestrelServerOptions>();
                        if (kestrelOptions != null)
                        {
                            options.AddServerHeader = kestrelOptions.AddServerHeader;
                            options.AllowSynchronousIO = kestrelOptions.AllowSynchronousIO; // false;
                            options.Limits.MinRequestBodyDataRate = null;
                            options.Limits.MinResponseDataRate = null;
                            //options.Limits.MaxRequestBodySize = null;
                            //options.Limits.MaxRequestBufferSize = null;
                            //options.Limits.MaxRequestHeaderCount = 1000;
                            //options.Limits.MaxRequestHeadersTotalSize = 327680;
                            //options.Limits.MaxRequestLineSize = 327680;
                            foreach (var property in typeof(KestrelServerLimits).GetProperties(BindingFlags.SetProperty))
                            {
                                var value = property.GetValue(kestrelOptions.Limits);
                                property.SetValue(options.Limits, value);
                            }
                        }
                    })

                   .UseStartup<Startup>();
                });
    }
}
