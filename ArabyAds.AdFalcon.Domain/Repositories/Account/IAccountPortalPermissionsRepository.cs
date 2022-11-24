using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArabyAds.AdFalcon.Domain.Model.Account;
using ArabyAds.Framework.Persistence;
using ArabyAds.AdFalcon.Domain.Model.Core;
using ArabyAds.AdFalcon.Domain.Common.Model.Core;

namespace ArabyAds.AdFalcon.Domain.Repositories
{
    public interface IAccountPortalPermissionsRepository : IKeyedRepository<AccountPortalPermissions, int>
    {

        List<PortalPermision> GetAccountAdPermissions(int accountId);
        bool checkAdPermissions(PortalPermissionsCode Code);
    }
}
