using ArabyAds.AdFalcon.Common.UserInfo;
using ArabyAds.AdFalcon.Web.Controllers.Utilities;
using ArabyAds.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
namespace ArabyAds.AdFalcon.Web.Controllers.Core.Security
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
