using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArabyAds.Framework.Security;
using ArabyAds.Framework.UserInfo;
using ArabyAds.Framework;
using ArabyAds.AdFalcon.Common.UserInfo;
using ArabyAds.AdFalcon.Domain.Services;
using ArabyAds.AdFalcon.Domain.Repositories;
using ArabyAds.AdFalcon.Domain.Model.Account;
using ArabyAds.AdFalcon.Domain.Common.Model.Account;

namespace ArabyAds.AdFalcon.Common.Grpc.UserInfo
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
                    UserDomainManager userProvider = new UserDomainManager(IoC.Instance.Resolve<IUserRepository>(), IoC.Instance.Resolve<IAccountRepository>());
                    User user = userProvider.GetUserByEmail(currentPrincipal.Identity.Name, false);
                    IUserAccountsRepository _UserAccountsRepositor = IoC.Instance.Resolve<IUserAccountsRepository>();
                    var AccountPortalPermissionsRepository = IoC.Instance.Resolve<IAccountPortalPermissionsRepository>();
                    var userac = _UserAccountsRepositor.Query(X => X.User.ID == user.ID && X.Account.ID == user.Account.ID).SingleOrDefault();
                    var Permissions = AccountPortalPermissionsRepository.GetAccountAdPermissions(user.Account.ID).Select(x =>(int) x.Code).ToArray();
                    if (user != null)
                        userInfo = new AdFalconUserInfo(user.FirstName, user.LastName, user.ID, user.Account.ID, user.Account.UserAgreementVersion, user.Account.AllowAPIAccess, user.Account.PrimaryUser.ID == user.ID || (userac.UserType == UserType.Primary), Permissions,(int)user.Account.AccountRole, user.GetVATValue(), user.EmailAddress, user.Company, (userac.UserType == UserType.ReadOnly));
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
            return new AdFalconUserInfo(string.Empty, string.Empty, null, null, null, false, false, new int[] { 0 },0,0, string.Empty, string.Empty,false);
        }
    }
}
