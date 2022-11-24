using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Noqoush.AdFalcon.Domain.Model.Account;
using Noqoush.Framework.Persistence;
using Noqoush.AdFalcon.Domain.Model.Core;
using Noqoush.AdFalcon.Domain.Common.Model.Core;

namespace Noqoush.AdFalcon.Domain.Repositories
{
    public interface IAccountPortalPermissionsRepository : IKeyedRepository<AccountPortalPermissions, int>
    {

        List<PortalPermision> GetAccountAdPermissions(int accountId);
        bool checkAdPermissions(PortalPermissionsCode Code);
    }
}
