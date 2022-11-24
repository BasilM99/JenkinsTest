using System;
using System.IO;
using System.Web;
using ArabyAds.AdFalcon.Services.Interfaces.Services.Campaign;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using ArabyAds.Framework;
using ArabyAds.Framework.Security;
using System.Security.Claims;
using System.Security.Principal;
using ArabyAds.AdFalcon.Services.Interfaces.Services.Core;


// ASP.NET Core middleware migrated from a handler



namespace ArabyAds.AdFalcon.Web.Controllers.Core
{
    public class AuthMiddleware
    {
        static SecurityManager securityProxy;
        static ITenantService tenantService;

        private RequestDelegate _next = null;
        // Must have constructor with this signature, otherwise exception at run time
        public AuthMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        private static void RegisterSecurityProxy()
        {
            securityProxy = new SecurityManager(IoC.Instance.Resolve<ISecurityService>());
            tenantService = IoC.Instance.Resolve<ITenantService>();
        }
        private async Task<bool> SetRequestTenant(string Domain)
        {
           
                Domain = "adfalcon.com";
           

            var tenantDto = IoC.Instance.Resolve<ITenantService>().GetTenantByDomain(Domain);
            ApplicationContext.CreateContext(tenantDto.Name, false, new Tenant { Name = tenantDto.Name, ID = tenantDto.ID, Domain = tenantDto.Domain, Code = tenantDto.Code });


            return await Task.FromResult(true);
        }
        public async Task Invoke(HttpContext context)
        {
            if (securityProxy == null)
            {
                RegisterSecurityProxy();
            }
            await SetRequestTenant(context.Request.GetDomain());

            if (context.User != null && !(context.User is ArabyAdsPrincipal))
            {
                if (context.User.Identity.IsAuthenticated)
                {
                    if (context.User.Identity is IIdentity)
                    {
                        ClaimsIdentity identity = (ClaimsIdentity)context.User.Identity;

                        //FormsAuthenticationTicket ticket = identity.;

                        //userSession = identity.;
                        //ClaimsIdentity user = User.Identity as ClaimsIdentity

                        string userSession = identity.FindFirst("UserToken").Value;
                        await securityProxy.BuildSecurityContext(userSession);
                    }
                }
            }
            await _next(context);

        }

    }

   
}

