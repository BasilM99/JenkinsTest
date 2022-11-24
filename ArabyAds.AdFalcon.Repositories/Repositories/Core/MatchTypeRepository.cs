using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArabyAds.AdFalcon.Domain.Model.AppSite;
using ArabyAds.AdFalcon.Domain.Repositories.Core;
using ArabyAds.Framework.Persistence;

namespace ArabyAds.AdFalcon.Persistence.Repositories.Core
{
    public class MatchTypeRepository : RepositoryBase<MatchType, int>, IMatchTypeRepository
    {
        public MatchTypeRepository(RepositoryImplBase<MatchType, int> repository)
            : base(repository)
        {


        }
    }
    
}
