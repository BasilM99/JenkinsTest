using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArabyAds.AdFalcon.Domain.Model.AppSite;
using ArabyAds.Framework.Persistence;
using ArabyAds.AdFalcon.Domain.Model.Core;

namespace ArabyAds.AdFalcon.Domain.Repositories.Core
{
    public interface ILookupRepository : IKeyedRepository<ManagedLookupBase, int>
    //public interface ILookupRepository<T,I> : IKeyedRepository<LookupBase<T,I>, int>
    {
    }
}
