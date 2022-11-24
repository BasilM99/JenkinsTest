using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArabyAds.AdFalcon.Domain.Model.AppSite;
using ArabyAds.AdFalcon.Domain.Model.Core;
using ArabyAds.Framework.Persistence;

namespace ArabyAds.AdFalcon.Domain.Repositories
{
    public interface IKeyWordRepository : IKeyedRepository<Keyword, int>
    {
        IEnumerable<Keyword> GetTop(int count);
    }
}
