using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArabyAds.Framework.Persistence;
using ArabyAds.AdFalcon.Domain.Model.Account;
using ArabyAds.Framework.DomainServices.AuditTrial;
using ArabyAds.AdFalcon.Domain.Model.Core.CostElement;

namespace ArabyAds.AdFalcon.Domain.Repositories.Account
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