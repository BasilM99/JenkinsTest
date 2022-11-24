using NHibernate;
using NHibernate.Criterion;
using NHibernate.Transform;
using ArabyAds.AdFalcon.Domain.Repositories;
using ArabyAds.Framework.DomainServices.AuditTrial;
using ArabyAds.Framework.Persistence;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;
using NHibernate.Criterion.Lambda;
using ArabyAds.AdFalcon.Domain.Repositories.Account;

namespace ArabyAds.AdFalcon.Persistence.Repositories.Account
{

    public class AccountSummaryRepository : RepositoryBase<ArabyAds.AdFalcon.Domain.Model.Account.AccountSummary, int>, IAccountSummaryRepository
    {
        public AccountSummaryRepository(RepositoryImplBase<ArabyAds.AdFalcon.Domain.Model.Account.AccountSummary, int> repository)
            : base(repository)
        {


        }


    }

}
