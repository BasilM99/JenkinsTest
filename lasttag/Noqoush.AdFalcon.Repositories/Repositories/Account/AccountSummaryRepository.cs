using NHibernate;
using NHibernate.Criterion;
using NHibernate.Transform;
using Noqoush.AdFalcon.Domain.Repositories;
using Noqoush.Framework.DomainServices.AuditTrial;
using Noqoush.Framework.Persistence;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;
using NHibernate.Criterion.Lambda;
using Noqoush.AdFalcon.Domain.Repositories.Account;

namespace Noqoush.AdFalcon.Persistence.Repositories.Account
{

    public class AccountSummaryRepository : RepositoryBase<Noqoush.AdFalcon.Domain.Model.Account.AccountSummary, int>, IAccountSummaryRepository
    {
        public AccountSummaryRepository(RepositoryImplBase<Noqoush.AdFalcon.Domain.Model.Account.AccountSummary, int> repository)
            : base(repository)
        {


        }


    }

}
