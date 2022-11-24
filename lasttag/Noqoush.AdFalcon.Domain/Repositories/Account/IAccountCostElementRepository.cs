using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Noqoush.Framework.Persistence;
using Noqoush.AdFalcon.Domain.Model.Account;
using Noqoush.Framework.DomainServices.AuditTrial;
using Noqoush.AdFalcon.Domain.Model.Core.CostElement;

namespace Noqoush.AdFalcon.Domain.Repositories.Account
{


    public interface IAccountCostElementRepository : IKeyedRepository<AccountCostElement, int>
    {
        List<AccountCostElement> GetAccountCostElements(int accountId);
    }


    public interface IAccountFeeRepository : IKeyedRepository<AccountFee, int>
    {
        List<AccountFee> GetAccountFees(int accountId);
    }
}