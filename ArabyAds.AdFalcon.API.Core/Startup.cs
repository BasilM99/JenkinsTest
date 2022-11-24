using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ArabyAds.AdFalcon.API.Controllers.Mapping;
using ArabyAds.AdFalcon.API.Controllers.Utilities;
using ArabyAds.Framework.ExceptionHandling;
using ArabyAds.Framework.Web.JsonConverters;

namespace ArabyAds.AdFalcon.API.Core
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            //exception handling
            ExceptionPolicy.RegisterHandler<Exception>(ExceptionHandlingPolicies.Framework, new ExceptionHandler());
            ExceptionPolicy.RegisterHandler<Exception>(ExceptionHandlingPolicies.Domain, new ExceptionHandler());
            ExceptionPolicy.RegisterHandler<Exception>(ExceptionHandlingPolicies.Threading, new ExceptionHandler(), false);
            ExceptionPolicy.RegisterHandler<Exception>(ExceptionHandlingPolicies.ServiceLayer, new ExceptionHandler());
            ExceptionPolicy.RegisterHandler<Exception>(ExceptionHandlingPolicies.UI, new LogExceptionHandler());
            services.AddHttpContextAccessor();
            services.AddControllersWithViews(x => {
                x.EnableEndpointRouting = false;
            }).AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new QuotedIntgerCoverter());
                options.JsonSerializerOptions.Converters.Add(new QuotedLongCoverter());
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter(allowIntegerValues: true));
                options.JsonSerializerOptions.Converters.Add(new DateTimeConverter());
                options.JsonSerializerOptions.Converters.Add(new EmptyStringDateTimeConverter());
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


            });
            MappingRegister.RegisterMapping();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            loggerFactory.AddLog4Net("log4net.config");
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();


            app.UseMvc(rb =>
            {

                rb.Routes.Add(new APIRoute(target: rb.DefaultHandler, domainTemplate: Config.APIDomainFormat, routeName: "APIRoute", routeTemplate: "{version}/{clientId}/{hash}/{controller}/{action}", new RouteValueDictionary(), new RouteValueDictionary(), new RouteValueDictionary(new { IsTest = false }), app.ApplicationServices.GetRequiredService<IInlineConstraintResolver>()));
                rb.Routes.Add(new APIRoute(target: rb.DefaultHandler, domainTemplate: Config.APIDomainFormat, routeName: "TestAPIRoute", routeTemplate: "test/{version}/{clientId}/{hash}/{controller}/{action}", new RouteValueDictionary(), new RouteValueDictionary(), new RouteValueDictionary(new { IsTest = false }), app.ApplicationServices.GetRequiredService<IInlineConstraintResolver>()));
                rb.Routes.Add(new Route(target: rb.DefaultHandler, routeName: "Default", routeTemplate: "{controller}/{action}", new RouteValueDictionary(), new RouteValueDictionary(), new RouteValueDictionary(), app.ApplicationServices.GetRequiredService<IInlineConstraintResolver>()));
            });
        }

    }
}

