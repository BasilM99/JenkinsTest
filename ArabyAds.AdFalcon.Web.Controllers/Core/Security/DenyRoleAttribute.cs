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
    public class DenyRoleAttribute : SecurityRoleAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (_RolesSplit.Length > 0 &&
                _RolesSplit.Any(role => OperationContext.Current.CurrentPrincipal.IsInRole(role)))
            {
                if (!DenyImpersonationOnly || (OperationContext.Current.UserInfo<AdFalconUserInfo>().AccountId != OperationContext.Current.UserInfo<AdFalconUserInfo>().OriginalAccountId))
                {

                    if (DenyImpersonationOnly && !(_authorizeRolesSplit.Length > 0 &&
                _authorizeRolesSplit.Any(role => OperationContext.Current.CurrentPrincipal.IsInRole(role))))
                    {
                        throw new Exception(ResourcesUtilities.GetResource("NotAuthorized", "Global"));
                    }
                    if (!DenyImpersonationOnly)
                    {

                        throw new Exception(ResourcesUtilities.GetResource("NotAuthorized", "Global"));
                    }

                }


            }

            if (AccountRoles != null && AccountRoles.Where(x => (int)x == OperationContext.Current.UserInfo<AdFalconUserInfo>().AccountRole).Count() > 0)
            {
                if (!((Config.IsAdmin||Config.IsAccountManager || Config.IsAdOps)  && (OperationContext.Current.UserInfo<AdFalconUserInfo>().AccountId != OperationContext.Current.UserInfo<AdFalconUserInfo>().OriginalAccountId)))
               {
                    throw new Exception(ResourcesUtilities.GetResource("NotAuthorized", "Global"));
                }

            }

            base.OnActionExecuting(filterContext);
        }
        public bool DenyImpersonationOnly { get; set; }


    }
}
