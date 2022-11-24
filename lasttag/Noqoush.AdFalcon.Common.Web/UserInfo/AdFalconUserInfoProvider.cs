using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Noqoush.AdFalcon.Services.Interfaces.Services.Account;
using Noqoush.Framework.Security;
using Noqoush.AdFalcon.Services.Interfaces.Services;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Account;
using Noqoush.Framework.UserInfo;
using Noqoush.Framework;
using Noqoush.AdFalcon.Common.UserInfo;
using Noqoush.AdFalcon.Domain.Repositories;

namespace Noqoush.AdFalcon.Common.Web.UserInfo
{
    public class AdFalconUserInfoProvider : IUserInfoProvider
    {
        
        public IUserInfo GetUserInfo()
        {
            NoqoushPrincipal currentPrincipal = SecurityContext.Current.CurrentPrincipal as NoqoushPrincipal;
            AdFalconUserInfo userInfo = null;

            if (currentPrincipal != null)
            {
                if (currentPrincipal.Identity.IsAuthenticated)
                {
                    IUserService userService = IoC.Instance.Resolve<IUserService>();
                  //  IUserAccountsRepository _UserAccountsRepositor = IoC.Instance.Resolve<IUserAccountsRepository>();
                    UserDto userDtoInfo = userService.GetUserByEmail(currentPrincipal.Identity.Name, false);
                    var IsSecondPrimaryUser = userService.IsUserSecondPrimaryUser(userDtoInfo.Id, userDtoInfo.AccountId);
                    var IsReadOnlyUser = userService.IsUserReadOnlyUser(userDtoInfo.Id, userDtoInfo.AccountId);
                    //var AccountAdPermissionsRepository = IoC.Instance.Resolve<IAccountAdPermissionsRepository>();
                    var Permissions = userService.GetAccountAdPermissions(userDtoInfo.AccountId).ToArray();

                    if (userDtoInfo != null)
                        userInfo = new AdFalconUserInfo(userDtoInfo.FirstName, userDtoInfo.LastName, userDtoInfo.Id, userDtoInfo.AccountId, userDtoInfo.UserAgreementVersion, userDtoInfo.AllowAPIAccess, userDtoInfo.IsPrimaryUser || IsSecondPrimaryUser, Permissions, userDtoInfo.AccountRole, userDtoInfo.VATValue, userDtoInfo.EmailAddress, userDtoInfo.Company, IsReadOnlyUser);
                    else
                        userInfo = BuildAnonymousUserInfo();
                }
                else
                    userInfo = BuildAnonymousUserInfo();
            }
            else
                throw new Exception("Principal should be filled before build the userinfo object");

            return userInfo;
        }

        private AdFalconUserInfo BuildAnonymousUserInfo()
        {
            return new AdFalconUserInfo(string.Empty, string.Empty, null, null, null, false,false,new int[] {0 },0,0, string.Empty, string.Empty,false);
        }
    }
}
