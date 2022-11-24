using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArabyAds.AdFalcon.Services.Interfaces.Services.Account;
using ArabyAds.Framework.Security;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Account;
using ArabyAds.Framework.UserInfo;
using ArabyAds.Framework;
using ArabyAds.AdFalcon.Common.UserInfo;
//using ArabyAds.AdFalcon.Domain.Repositories;

namespace ArabyAds.AdFalcon.Common.Web.UserInfo
{
    public class AdFalconUserInfoProvider : IUserInfoProvider
    {
        
        public IUserInfo GetUserInfo()
        {
            ArabyAdsPrincipal currentPrincipal = SecurityContext.Current.CurrentPrincipal as ArabyAdsPrincipal;
            AdFalconUserInfo userInfo = null;

            if (currentPrincipal != null)
            {
                if (currentPrincipal.Identity.IsAuthenticated)
                {
                    IUserService userService = IoC.Instance.Resolve<IUserService>();
                  //  IUserAccountsRepository _UserAccountsRepositor = IoC.Instance.Resolve<IUserAccountsRepository>();
                    UserDto userDtoInfo = userService.GetUserByEmail(new Services.Interfaces.Messages.CheckUserEmailRequest { EmailAddress = currentPrincipal.Identity.Name, CheckPendingEmail = false} );
                    var IsSecondPrimaryUser = userService.IsUserSecondPrimaryUser(new Services.Interfaces.Messages.UserAccountMessage { AccountId = userDtoInfo.AccountId, UserId = userDtoInfo.Id }).Value;
                    var IsReadOnlyUser = userService.IsUserReadOnlyUser(new Services.Interfaces.Messages.UserAccountMessage { AccountId = userDtoInfo.AccountId, UserId = userDtoInfo.Id } ).Value;
                    //var AccountAdPermissionsRepository = IoC.Instance.Resolve<IAccountAdPermissionsRepository>();
                    var Permissions = userService.GetAccountAdPermissions(new ValueMessageWrapper<int> { Value= userDtoInfo.AccountId }).ToArray();

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
