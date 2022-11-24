using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Noqoush.AdFalcon.Web.Controllers.Utilities;
using Noqoush.Framework;
using Noqoush.AdFalcon.Web.Controllers.Core.Security;


namespace Noqoush.AdFalcon.Web.Controllers.Core
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class AuthorizeRoleAttribute : SecurityRoleAttribute
    {

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (_RolesSplit.Length > 0 &&
                !_RolesSplit.Any(role => OperationContext.Current.CurrentPrincipal.IsInRole(role)))
            {
                throw new Exception(ResourcesUtilities.GetResource("NotAuthorized", "Global"));
                
            }
            base.OnActionExecuting(filterContext);
        }

    }
}
