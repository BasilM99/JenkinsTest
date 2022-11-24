using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using ArabyAds.Framework;
using ArabyAds.Framework.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Routing;
using System.Threading;
using System.Globalization;
using ArabyAds.AdFalcon.Web.Controllers.Utilities;
using ArabyAds.Framework.Utilities;

namespace ArabyAds.AdFalcon.Web.Controllers.Core
{
    public class CurrentUserProfileFilter : IAuthorizationFilter
    {
        static SecurityManager securityProxy;

        public void OnAuthorization(AuthorizationFilterContext filterContext)
        {
            if (filterContext.HttpContext.User is ArabyAdsPrincipal) return;
            string userSession = string.Empty;
            if (securityProxy == null)
            {
                RegisterSecurityProxy();
            }
            if (filterContext.HttpContext.User != null)
            {
                if (filterContext.HttpContext.User.Identity.IsAuthenticated)
                {
                    if (filterContext.HttpContext.User.Identity is IIdentity)
                    {
                        ClaimsIdentity identity = (ClaimsIdentity)filterContext.HttpContext.User.Identity;

                        //FormsAuthenticationTicket ticket = identity.;

                        //userSession = identity.;
                        //ClaimsIdentity user = User.Identity as ClaimsIdentity

                        userSession = identity.FindFirst("UserToken").Value;
                    }
                }
            }

            securityProxy.BuildSecurityContext(userSession).ConfigureAwait(false).GetAwaiter().GetResult();

        }



        private static void RegisterSecurityProxy()
        {
            securityProxy = new SecurityManager(IoC.Instance.Resolve<ISecurityService>());
        }

    }




}
