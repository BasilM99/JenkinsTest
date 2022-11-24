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
using Noqoush.AdFalcon.Domain.Model.Account;

namespace Noqoush.AdFalcon.Persistence.Repositories.Core
{
    public class BuyerRepository : RepositoryBase<Buyer, int>, IBuyerRepository
    {
        public BuyerRepository(RepositoryImplBase<Buyer, int> repository)
            : base(repository)
        {
        }
    }
}
