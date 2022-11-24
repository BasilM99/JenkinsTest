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
    class AccountFundTransStatusRepository : RepositoryBase<AccountFundTransStatus, int>, IAccountFundTransStatusRepository
    {
        public AccountFundTransStatusRepository(RepositoryImplBase<AccountFundTransStatus, int> repository)
            : base(repository)
        {
        }
       
    }
}
