using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ArabyAds.AdFalcon.Services.Interfaces.Services.Account;
using ArabyAds.AdFalcon.Services.Mapping;
using ArabyAds.AdFalcon.Services.Services.Account;
using ArabyAds.Framework;
using ArabyAds.Framework.Behaviors;
using ArabyAds.Framework.Caching;
using ArabyAds.Framework.EventBroker;
using ArabyAds.Framework.ExceptionHandling;
using ArabyAds.Framework.Grpc;
using ArabyAds.Framework.Grpc.Behaviors;
using ArabyAds.Framework.Persistence;
using ArabyAds.Framework.Persistence.NHibernate;
using ArabyAds.Framework.Security;
using org.apache.zookeeper;
using ProtoBuf.Grpc.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.DataProtection;
using ArabyAds.Framework.Utilities.Encryptyion;
using Microsoft.EntityFrameworkCore;
using ArabyAds.Framework.Utilities;
using ArabyAds.AdFalcon.Persistence.EF;

namespace ArabyAds.AdFalcon.Services
{
    public class Startup
    {

        public void ConfigureServices(IServiceCollection services)
        {
         
            // Add a DbContext to store your Database Keys
            services.AddDbContext<MyKeysContext>(options =>
                options.UseMySQL(
                   JsonConfigurationManager.ConnectionStrings["MyKeysConnection"], b => b.MigrationsAssembly("ArabyAds.AdFalcon.Services")));

            // using Microsoft.AspNetCore.DataProtection;
            services.AddDataProtection()
                .SetApplicationName("AdFalcon").PersistKeysToDbContext<MyKeysContext>();

        


            services.AddHttpContextAccessor();
            


            //by mosab copied as is from global.asax
            IoC.Instance.GetType();
            EventBroker.Instance.GetType();
            Assembly[] assemblies = new Assembly[] { Assembly.Load("ArabyAds.AdFalcon.Persistence"), Assembly.Load("ArabyAds.Framework.DomainServices") };
            var UoWFactory = new NHibernateUnitOfWorkFactory();
            UoWFactory.Initialize(assemblies);
            UnitOfWork.SetUnitOfWorkFactory(UoWFactory);
            BehaviorInvoker.AddBehavior(new AuthenticationRequiredBehavior());
            BehaviorInvoker.AddIgnoredBehaviorType(typeof(CachableBehavior));
            MappingRegister.RegisterMapping();

            //by mosab to do add data protection
            //by mossb to do add diagnostics tracing 
            services.AddProxiedGrpcServices(new Assembly[] { typeof(IAccountService).Assembly }, new Assembly[] { typeof(AccountService).Assembly }, typeof(PersistenceManager), typeof(BehaviorInvoker));
            services.AddCodeFirstGrpc(opt => 
            {
                opt.MaxReceiveMessageSize = Int32.MaxValue;
                opt.MaxSendMessageSize = Int32.MaxValue;
                opt.Interceptors.Add<GrpcAuthorizationPolicy>();
                opt.Interceptors.Add<ServiceOperationContextPolicy>();
                opt.Interceptors.Add<ApplicationContextPolicy>();
            });

            //exception handling
            ExceptionPolicy.RegisterHandler<Exception>(ExceptionHandlingPolicies.Framework, new ExceptionHandler());
            ExceptionPolicy.RegisterHandler<Exception>(ExceptionHandlingPolicies.Domain, new ExceptionHandler());
            ExceptionPolicy.RegisterHandler<Exception>(ExceptionHandlingPolicies.Threading, new ExceptionHandler(), false);
            ExceptionPolicy.RegisterHandler<Exception>(ExceptionHandlingPolicies.ServiceLayer, new LogExceptionHandler());
            ExceptionPolicy.RegisterHandler<Exception>(ExceptionHandlingPolicies.UI, new ExceptionHandler());
            //var instance = new ArabyAds.Framework.Utilities.Cryptography(services.b);

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseRouting();
            loggerFactory.AddLog4NetProvider("log4net.config");
            app.UseEndpoints(endpoints =>
            {

                endpoints.MapGrpcServices(new Assembly[] { typeof(IAccountService).Assembly });
                endpoints.MapGet("/", async context =>
                {

                    await context.Response.WriteAsync("Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");
                });
            });


            var dataProtectionProvider = app.ApplicationServices.GetDataProtectionProvider();
            var instance = new ArabyAds.Framework.Utilities.Cryptography(dataProtectionProvider);
            // create an instance of MyClass using the service provider
        }
    }
}
