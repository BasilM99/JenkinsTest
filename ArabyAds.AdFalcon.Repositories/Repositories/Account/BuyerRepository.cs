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
using ArabyAds.AdFalcon.Domain.Model.Account;

namespace ArabyAds.AdFalcon.Persistence.Repositories.Core
{
    public class BuyerRepository : RepositoryBase<Buyer, int>, IBuyerRepository
    {
        public BuyerRepository(RepositoryImplBase<Buyer, int> repository)
            : base(repository)
        {
        }
    }
}
