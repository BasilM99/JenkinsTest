using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Noqoush.AdFalcon.Domain.Model.AppSite;
using Noqoush.AdFalcon.Domain.Repositories;
using Noqoush.Framework.Persistence;

namespace Noqoush.AdFalcon.Repositories.Repositories
{
    public class AppSiteTypeRepository : RepositoryBase<AppSiteType, int>, IAppSiteTypeRepository
    {
        public AppSiteTypeRepository(RepositoryImplBase<AppSiteType, int> repository)
            : base(repository)
        {
        }
    }
}
