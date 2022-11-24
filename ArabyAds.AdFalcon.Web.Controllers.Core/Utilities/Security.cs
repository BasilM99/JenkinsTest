using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using ArabyAds.AdFalcon.Web.Controllers.Utilities;
using ArabyAds.Framework;
using ArabyAds.AdFalcon.Common.UserInfo;
using ArabyAds.AdFalcon.Domain.Common.Model.Account;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ArabyAds.AdFalcon.Web.Controllers.Core.Utilities
{
    public class Security
    {
        public static void CheckDenyRoleSecurity(AccountRole[] AccountRoles, string[] _RolesSplit , string[] _authorizeRolesSplit, bool DenyImpersonationOnly=false)
        {
            if (_RolesSplit == null)
                _RolesSplit = new string[0];
            if (_authorizeRolesSplit == null)
                _authorizeRolesSplit = new string[0];
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
                if (!((Config.IsAdmin || Config.IsAccountManager || Config.IsAdOps) && (OperationContext.Current.UserInfo<AdFalconUserInfo>().AccountId != OperationContext.Current.UserInfo<AdFalconUserInfo>().OriginalAccountId)))
                {
                    throw new Exception(ResourcesUtilities.GetResource("NotAuthorized", "Global"));
                }

            }


        }
        public static void CustomAuthorize()
        {
            if(!(OperationContext.Current.CurrentPrincipal.Identity.IsAuthenticated))
                throw new Exception(ResourcesUtilities.GetResource("NotAuthorized", "Global"));
        }
        public static void AuthorizeRole(string[] _RolesSplit)
        {
            if (_RolesSplit.Length > 0 &&
                !_RolesSplit.Any(role => OperationContext.Current.CurrentPrincipal.IsInRole(role)))
            {
                throw new Exception(ResourcesUtilities.GetResource("NotAuthorized", "Global"));

            }
        }
        public static void AllowRole(AccountRole[] AccountRoles)
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

        }
      
}
}
