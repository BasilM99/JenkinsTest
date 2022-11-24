using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;
using System.Linq;

namespace ArabyAds.AdFalcon.Web.React
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

            System.IO.Directory.SetCurrentDirectory(System.AppDomain.CurrentDomain.BaseDirectory);

            // In production, the React files will be served from this directory
            services.AddResponseCompression(options =>
            {
                options.EnableForHttps = true;
            });
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/build";

            });
            //services.AddSpaStaticFiles();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            System.IO.Directory.SetCurrentDirectory(System.AppDomain.CurrentDomain.BaseDirectory);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                //app.UseHsts();
            }
            app.UseResponseCaching();
            //            app.UseRewriter(new RewriteOptions()
            //.Add(RewriteRules.RedirectRequests)
            //);
            app.Use(async (context, next) =>
            {
                var url = context.Request.Path.Value;

                var request = url;
                var path = url;

                var userLangs = context.Request.Headers.ContainsKey("Accept-Language") && !string.IsNullOrWhiteSpace(context.Request.Headers["Accept-Language"]) ? context.Request.Headers["Accept-Language"].ToString() : "en";
                var firstLang = userLangs.Split(',').FirstOrDefault();
                var defultCulture = string.IsNullOrWhiteSpace(firstLang) ? "en" : firstLang.Substring(0, 2);

                // Add your conditions of redirecting
                if ((path.Split("/")[1] != "en") && (path.Split("/")[1] != "ar"))// If the url does not contain culture
                {
                    if (!request.Contains("Common/Resources") && (!context.Request.Headers.ContainsKey("X-ADFALCON-API")) && (string.IsNullOrEmpty(context.Request.Headers["X-Requested-With"])) && context.Request.Method.ToLower() != "post" && !url.Contains("assets") && !url.Contains(".ashx") && !url.Contains(".jpg") && !url.Contains(".gif") && !url.Contains(".jpeg") && !url.Contains(".png") && !url.Contains(".css") && !url.Contains(".js") && !url.Contains("Document"))
                    {
                        
                        context.Request.Path = $"/{defultCulture}{ Microsoft.AspNetCore.Http.Extensions.UriHelper.GetEncodedPathAndQuery(context.Request)}";
                        context.Response.Redirect(context.Request.Path,true);
                        return;
                    }

                }


                // Rewrite to index

                await next();
            });




            //app.UseFileServer();
            PhysicalFileProvider fileProvider = new PhysicalFileProvider(
    Path.Combine(Directory.GetCurrentDirectory(), @"ClientApp"));
            DefaultFilesOptions defoptions = new DefaultFilesOptions();
            defoptions.DefaultFileNames.Clear();
            defoptions.FileProvider = fileProvider;
            defoptions.DefaultFileNames.Add("index.html");
            app.UseDefaultFiles(defoptions);
            app.UseDefaultFiles();


            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "ClientApp")),
                HttpsCompression = Microsoft.AspNetCore.Http.Features.HttpsCompressionMode.Compress,
                
                OnPrepareResponse = (context) =>
                {



                  

                    var extension = System.IO.Path.GetExtension(context.File.PhysicalPath);
                    var headers = context.Context.Response.GetTypedHeaders();
                    string contentType = headers.ContentType.MediaType.Value;
                    if (contentType == "application/x-gzip")
                    {
                        if (context.File.Name.EndsWith("js.gz"))
                        {
                            headers.ContentType = new Microsoft.Net.Http.Headers.MediaTypeHeaderValue("application/javascript");

                        }
                        else if (context.File.Name.EndsWith("css.gz"))
                        {
                            headers.ContentType = new Microsoft.Net.Http.Headers.MediaTypeHeaderValue("text/css");

                        }
                        var cachedates = 7;
                        context.Context.Response.Headers.Add("Content-Encoding", "gzip");
                        headers.CacheControl = new Microsoft.Net.Http.Headers.CacheControlHeaderValue
                        {
                            Public = true,
                            MaxAge = TimeSpan.FromDays(7),
                            MustRevalidate = true,

                        };
                        headers.Expires = ArabyAds.Framework.Utilities.Environment.GetServerTime().AddDays(7);



                    }

                

                }
            });




            app.MapWhen(ctx => {
                if (ctx.Request.Path.Value.Split("/").Length>=3)
                {
                 
                    return true;
                }
                return false;
            }, app1 =>
            {
                app1.UseHttpsRedirection();
                app1.UseSpa(spa =>
                {
                    // To learn more about options for serving an Angular SPA from ASP.NET Core,
                    // see https://go.microsoft.com/fwlink/?linkid=864501

                    spa.Options.DefaultPageStaticFileOptions = new StaticFileOptions()
                    {
                        FileProvider =
                 new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "ClientApp/build"))
                    };

                    //spa.Options.SourcePath = "ClientApp";




                });
            });

            //app.UseSpa(spa =>
            //{

            //    spa.Options.SourcePath = "ClientApp";

            //    /*if (env.IsDevelopment())
            //    {
            //        spa.UseReactDevelopmentServer(npmScript: "start");
            //    }*/
            //});

            //app.Map("/ar/app", app1 =>
            //{
            //    app1.UseHttpsRedirection();
            //    app1.UseSpa(spa =>
            //    {
            //        // To learn more about options for serving an Angular SPA from ASP.NET Core,
            //        // see https://go.microsoft.com/fwlink/?linkid=864501

            //        spa.Options.DefaultPageStaticFileOptions = new StaticFileOptions()
            //        {
            //            FileProvider =
            //     new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "ClientApp/build"))
            //        };

            //        spa.Options.SourcePath = "ClientApp/build";




            //    });
            //});
            //app.Map("/en/app", app1 =>
            //{
            //    app1.UseHttpsRedirection();
            //    app1.UseSpa(spa =>
            //    {
            //        // To learn more about options for serving an Angular SPA from ASP.NET Core,
            //        // see https://go.microsoft.com/fwlink/?linkid=864501

            //        spa.Options.DefaultPageStaticFileOptions = new StaticFileOptions()
            //        {
            //            FileProvider =
            //     new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "ClientApp/build"))
            //        };

            //        spa.Options.SourcePath = "ClientApp/build";




            //    });
            //});

            //app.Map("/app", app1 =>
            //{
            //    app1.UseHttpsRedirection();
            //    app1.UseSpa(spa =>
            //    {
            //        // To learn more about options for serving an Angular SPA from ASP.NET Core,
            //        // see https://go.microsoft.com/fwlink/?linkid=864501

            //        spa.Options.DefaultPageStaticFileOptions = new StaticFileOptions()
            //        {
            //            FileProvider =
            //     new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "ClientApp/build"))
            //        };

            //        spa.Options.SourcePath = "ClientApp/build";




            //    });
            //});


        }
    }
}
