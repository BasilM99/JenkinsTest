using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Noqoush.AdFalcon.Web.Controllers.Utilities;
using Noqoush.Framework;
using Noqoush.AdFalcon.Common.UserInfo;
using Noqoush.AdFalcon.Domain.Common.Model.Account;

namespace Noqoush.AdFalcon.Web.Controllers.Core.Security
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class AllowRoleAttribute : SecurityRoleAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!Config.IsAdOpsAdmin)
            {
                if (OperationContext.Current.UserInfo<AdFalconUserInfo>().ImpersonatedAccount == null && !Config.IsAdmin && AccountRoles != null && AccountRoles.Where(x => (int)x == OperationContext.Current.UserInfo<AdFalconUserInfo>().AccountRole).Count() == 0)
                {
                    throw new Exception(ResourcesUtilities.GetResource("NotAuthorized", "Global"));

                }
            }

           

            if (OperationContext.Current.UserInfo<AdFalconUserInfo>().ImpersonatedAccount != null && !Config.IsAdmin && AccountRoles != null && AccountRoles.Where(x => (int)x == OperationContext.Current.UserInfo<AdFalconUserInfo>().ImpersonatedAccount.AccountRole).Count() == 0)
            {


            }
            base.OnActionExecuting(filterContext);
        }
    }
}
