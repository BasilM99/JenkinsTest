using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArabyAds.Framework.Persistence;
using ArabyAds.AdFalcon.Domain.Model.Account;
using ArabyAds.AdFalcon.Domain.Repositories;

namespace ArabyAds.AdFalcon.Persistence.Repositories
{
    class AccountFundTransTypeRepository : RepositoryBase<AccountFundTransType, int>, IAccountFundTransTypeRepository
    {
        public AccountFundTransTypeRepository(RepositoryImplBase<AccountFundTransType, int> repository)
            : base(repository)
        {
        }

    }
}
