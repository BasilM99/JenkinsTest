using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArabyAds.AdFalcon.Domain.Model.AppSite;
using ArabyAds.Framework.Persistence;

namespace ArabyAds.AdFalcon.Domain.Repositories
{
   


    public interface ISubAppsiteRepository : IKeyedRepository<SubAppsite, int>
    {
        IEnumerable<SubAppsiteTransfomer> GetSubAppSitesQuery(AllAppSiteCriteria criteria, out int Count);
    }
}
