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
using System.Text.Json.Serialization;
using Microsoft.Extensions.Configuration;

namespace ArabyAds.AdFalcon.Web.Core
{
 

    
 

  
    public class WebStartup:CommonStartup
    {

        public WebStartup(IWebHostEnvironment env)
        {
            //Configuration = configuration;
            _env = env;
        }
        public void ConfigureServices(IServiceCollection services)
        {
            try
            {
            //    AppContext.SetSwitch(
            //"System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);


                CommonServices(services);
                    var startupAssembly = typeof(UserController).Assembly;
                services
    .AddControllersWithViews(options =>
    {


        //        var policy = new AuthorizationPolicyBuilder()
        //.RequireAuthenticatedUser()
        //.Build();


        options.Filters.Add(new LoclizationFilter());
        options.Filters.Add(new CurrentUserProfileFilter());

        // options.Filters.Add(new AuthorizeFilter(policy));
        options.Filters.Add(new AddCookieResultServiceFilter());       // An instance
        options.EnableEndpointRouting = false;


    }).AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new QuotedIntgerCoverter());
        options.JsonSerializerOptions.Converters.Add(new QuotedLongCoverter());
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter(allowIntegerValues: true));
        options.JsonSerializerOptions.Converters.Add(new DateTimeConverter());
        options.JsonSerializerOptions.Converters.Add(new QuotedDecimalCoverter());
        options.JsonSerializerOptions.Converters.Add(new QuotedDoubleCoverter());
        options.JsonSerializerOptions.Converters.Add(new QuotedFloatCoverter());

        options.JsonSerializerOptions.Converters.Add(new QuotedNullableIntgerCoverter());
        options.JsonSerializerOptions.Converters.Add(new QuotedNullableLongCoverter());
        options.JsonSerializerOptions.Converters.Add(new QuotedNullableDecimalCoverter());
        options.JsonSerializerOptions.Converters.Add(new QuotedNullableDoubleCoverter());
        options.JsonSerializerOptions.Converters.Add(new QuotedNullableFloatCoverter());
        options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
        options.JsonSerializerOptions.PropertyNamingPolicy = null;
        //  options.JsonSerializerOptions.AllowTrailingCommas = true;


    })
    .AddApplicationPart(startupAssembly)
    .AddControllersAsServices();
                services.AddScoped<AddCookieResultServiceFilter>();
                services.AddScoped<CurrentUserProfileFilter>();
                services.AddScoped<LoclizationFilter>();

               

            }
            catch (Exception ex)
            {

                throw ex;
            }

        }


      
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            try
            {
           //     AppContext.SetSwitch(
           //"System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);

                CommonConfigure(app, env, loggerFactory, configuration);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


       
       
    }
}
