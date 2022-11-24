using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using ArabyAds.AdFalcon.Web.Controllers.Core;
//using ArabyAds.AdFalcon.Web.Controllers.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace ArabyAds.AdFalcon.Web.Core
{
    public class Program
    {
        public static void Main(string[] args)
        {
            AppContext.SetSwitch("Microsoft.AspNetCore.Routing.UseCorrectCatchAllBehavior",
                         true);
            AppContext.SetSwitch("Microsoft.AspNetCore.Server.Kestrel.EnableWindows81Http2", true);
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseWindowsService()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    //var assemblyName = typeof(WebApplicationBase).GetTypeInfo().Assembly.FullName;

                    webBuilder.UseKestrel((builderContext, options) =>
                    {
                        var kestrelSection = builderContext.Configuration.GetSection("Kestrel");
                        options.Configure(kestrelSection);
                        var kestrelOptions = kestrelSection.Get<KestrelServerOptions>();
                        if (kestrelOptions != null)
                        {
                            options.AllowSynchronousIO = kestrelOptions.AllowSynchronousIO;

                        }

                      
                    });
                  //  webBuilder.UseContentRoot(AppContext.BaseDirectory);
                    webBuilder.UseStartup<WebStartup>();
                      
                    //AhmadComment
                    //webBuilder.UseStartup<Startup>();
                });
    }
}
                                                                             ;