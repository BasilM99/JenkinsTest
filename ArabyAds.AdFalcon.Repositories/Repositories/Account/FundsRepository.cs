using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArabyAds.Framework.Persistence;
using ArabyAds.AdFalcon.Domain.Repositories;
using ArabyAds.AdFalcon.Domain.Model.Account;

namespace ArabyAds.AdFalcon.Persistence.Repositories
{
    class FundsRepository : RepositoryBase<AccountFundTransHistory, int>, IFundsRepository
    {
        public FundsRepository(RepositoryImplBase<AccountFundTransHistory, int> repository)
            : base(repository)
        {
        }
    }
}
