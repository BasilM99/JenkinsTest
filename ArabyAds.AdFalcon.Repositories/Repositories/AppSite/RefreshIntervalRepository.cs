using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArabyAds.AdFalcon.Domain.Model.AppSite;
using ArabyAds.AdFalcon.Domain.Repositories;
using ArabyAds.Framework.Persistence;

namespace ArabyAds.AdFalcon.Persistence.Repositories
{
    public class RefreshIntervalRepository : RepositoryBase<AppSiteRefreshMode, int>, IRefreshIntervalRepository
    {
        public RefreshIntervalRepository(RepositoryImplBase<AppSiteRefreshMode, int> repository)
            : base(repository)
        {
        }
    }
}
