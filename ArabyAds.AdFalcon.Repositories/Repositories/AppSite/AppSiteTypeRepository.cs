using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArabyAds.AdFalcon.Domain.Model.AppSite;
using ArabyAds.AdFalcon.Domain.Repositories;
using ArabyAds.Framework.Persistence;

namespace ArabyAds.AdFalcon.Repositories.Repositories
{
    public class AppSiteTypeRepository : RepositoryBase<AppSiteType, int>, IAppSiteTypeRepository
    {
        public AppSiteTypeRepository(RepositoryImplBase<AppSiteType, int> repository)
            : base(repository)
        {
        }
    }
}
