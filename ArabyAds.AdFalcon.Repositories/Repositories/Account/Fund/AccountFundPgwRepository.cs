using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArabyAds.AdFalcon.Domain.Model.Account;
using ArabyAds.AdFalcon.Domain.Model.AppSite;
using ArabyAds.AdFalcon.Domain.Repositories;
using ArabyAds.Framework.Persistence;
using NHibernate;


namespace ArabyAds.AdFalcon.Persistence.Repositories
{
    class AccountFundPgwRepository : RepositoryBase<AccountFundPgw, int>, IAccountFundPgwRepository
    {
        public AccountFundPgwRepository(RepositoryImplBase<AccountFundPgw, int> repository)
            : base(repository)
        {
        }

    }
}
