using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Builder;
//using System.Web.Security;
using ArabyAds.AdFalcon.Web.Controllers.Controllers;
using ArabyAds.AdFalcon.Web.Controllers.Utilities;
using ArabyAds.Framework;
using ArabyAds.Framework.Security;
//using Combres;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using ArabyAds.AdFalcon.Web.Controllers.Handler;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using ITempDataProvider = Microsoft.AspNetCore.Mvc.ViewFeatures.ITempDataProvider;
using Microsoft.AspNetCore.Mvc.Routing;
using ArabyAds.AdFalcon.Web.Controllers.Core;
using Microsoft.AspNetCore.Rewrite;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Hosting;
using ArabyAds.AdFalcon.Web.Controllers.Core.Core;
using ArabyAds.Framework.Utilities;
using ArabyAds.Framework.Web;
using ArabyAds.Framework.Web.JsonConverters;

namespace ArabyAds.AdFalcon.Web.Controllers.Core.Core
{
    public class ConfigureMvcOptions : IConfigureOptions<MvcOptions>
    {
        private readonly IWebHostEnvironment _env;
        public ConfigureMvcOptions(IWebHostEnvironment env)
        {
            _env = env;
        }

        public void Configure(MvcOptions options)
        {
            var portNo = 443;
            int.TryParse(JsonConfigurationManager.AppSettings["HttpsPortNo"], out portNo);
            options.SslPort = portNo;

        }
    }
}
