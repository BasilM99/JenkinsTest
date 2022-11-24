using NHibernate;
using ArabyAds.AdFalcon.Domain.Common.Model.Core;
using ArabyAds.AdFalcon.Domain.Model.Core;
using ArabyAds.AdFalcon.Domain.Model;
using ArabyAds.AdFalcon.Domain.Repositories.Campaign;
using ArabyAds.AdFalcon.Domain.Repositories.Core;
using ArabyAds.Framework.Persistence;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;

namespace ArabyAds.AdFalcon.Persistence.Repositories.Core
{
    public class PartyRepository : RepositoryBase<Party, int>, IPartyRepository
    {
        public PartyRepository(RepositoryImplBase<Party, int> repository)
            : base(repository)
        {


        }

        public IEnumerable<T> Query<T>(Expression<Func<T, bool>> filter) where T : Party
        {
            var rootQuery  = UnitOfWork.Current.EntitySet<T>().Where(filter);
            return rootQuery.ToList<T>();
        }

        public int FilteredCount<T>(Expression<Func<T, bool>> filter) where T : Party
        {
            var rootQuery = UnitOfWork.Current.EntitySet<T>().Where(filter);
            return rootQuery.Count();
        }

        public IEnumerable<T> Query<T>(Expression<Func<T, bool>> filter, int pageIndex, int pageCount, Expression<Func<T, object>> orderByExpression, bool ascending) where T : Party 
        {

            var query = UnitOfWork.Current.EntitySet<T>().Where(filter);
            if (ascending)
                query = query.OrderBy(orderByExpression);
            else
                query = query.OrderByDescending(orderByExpression);

            query = query.Take(pageCount).Skip(pageIndex * pageCount);

            return query.ToList<T>();
        }
    }
}
