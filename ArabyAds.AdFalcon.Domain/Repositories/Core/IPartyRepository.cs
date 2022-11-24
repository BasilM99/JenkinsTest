using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArabyAds.AdFalcon.Domain.Model.Account.Discount;
using ArabyAds.AdFalcon.Domain.Model.AppSite;
using ArabyAds.AdFalcon.Domain.Model.Core.CostElement;
using ArabyAds.Framework.Persistence;
using ArabyAds.AdFalcon.Domain.Model.Core;
using ArabyAds.AdFalcon.Domain.Common.Model.Core;
using System.Linq.Expressions;

namespace ArabyAds.AdFalcon.Domain.Repositories.Core
{
    public interface IPartyRepository : IKeyedRepository<Party, int>
    {
        IEnumerable<T> Query<T>(Expression<Func<T, bool>> filter) where T : Party;
        IEnumerable<T> Query<T>(Expression<Func<T, bool>> filter, int pageIndex, int pageCount, Expression<Func<T, object>> orderByExpression, bool ascending) where T : Party;
        int FilteredCount<T>(Expression<Func<T, bool>> filter) where T : Party;
    }
}
