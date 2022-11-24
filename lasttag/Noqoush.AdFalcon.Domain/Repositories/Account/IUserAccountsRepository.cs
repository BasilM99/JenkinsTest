using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Noqoush.Framework.Persistence;
using Noqoush.AdFalcon.Domain.Model.Account;
using Noqoush.Framework.DomainServices.AuditTrial;

namespace Noqoush.AdFalcon.Domain.Repositories
{
    public interface IUserAccountsRepository : IKeyedRepository<UserAccounts, int>
    {
        
    }
}
