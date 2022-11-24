using Noqoush.AdFalcon.Common.UserInfo;
using Noqoush.AdFalcon.Web.Controllers.Utilities;
using Noqoush.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Noqoush.AdFalcon.Web.Controllers.Core.Security
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]

    public class DenyNonPrimaryRoleAttribute : SecurityRoleAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {

            if (OperationContext.Current.UserInfo<AdFalconUserInfo>().ImpersonatedAccount == null && !OperationContext.Current.UserInfo<AdFalconUserInfo>().IsPrimaryUser)
            {
                throw new Exception(ResourcesUtilities.GetResource("NotAuthorized", "Global"));
            }

            base.OnActionExecuting(filterContext);
        }
    }
}
