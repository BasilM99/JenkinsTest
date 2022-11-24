using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using ArabyAds.AdFalcon.Web.Controllers.Utilities;
using ArabyAds.Framework;
using ArabyAds.AdFalcon.Web.Controllers.Core.Security;
using ArabyAds.AdFalcon.Domain.Common.Model.Core;
using ArabyAds.AdFalcon.Services.Interfaces.Services.Account;
using Microsoft.AspNetCore.Mvc.Filters;
namespace ArabyAds.AdFalcon.Web.Controllers.Core
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class PermissionsAuthorize : SecurityRoleAttribute
    {
        public PortalPermissionsCode Permission { get; set; }
        protected IAccountService _accountService = IoC.Instance.Resolve<IAccountService>();

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {

            if (!Config.IsAdmin)
            {
                if ((!_accountService.checkAdPermissions(new ValueMessageWrapper<Domain.Common.Model.Core.PortalPermissionsCode> { Value = Permission }).Value  ) && !(_RolesSplit.Length > 0 &&
               _RolesSplit.Any(role => OperationContext.Current.CurrentPrincipal.IsInRole(role))))
                {
                    throw new Exception(ResourcesUtilities.GetResource("NotAuthorized", "Global"));
                }
            }
         
          
            base.OnActionExecuting(filterContext);
        }

    }
}
