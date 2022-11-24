using ArabyAds.AdFalcon.Domain.Model.Campaign;
using ArabyAds.AdFalcon.Domain.Repositories.Campaign;
using ArabyAds.AdFalcon.Domain.Repositories.Core;
using ArabyAds.Framework.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArabyAds.AdFalcon.Persistence.Repositories.Campaign
{
   

    public class AudienceSegmentEventMapRepository : RepositoryBase<AudienceSegmentEventMap, int>, IAudienceSegmentEventMapRepository
    {
        public AudienceSegmentEventMapRepository(RepositoryImplBase<AudienceSegmentEventMap, int> repository)
            : base(repository)
        {
        }
    }
}
