using ArabyAds.AdFalcon.Domain.Repositories.Campaign;
using ArabyAds.Framework.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArabyAds.AdFalcon.Persistence.Repositories.Campaign
{
    class AdCreativeAttributeRepository : RepositoryBase<Domain.Model.Campaign.AdCreativeAttribute, int>, IAdCreativeAttributeRepository
    {
        public AdCreativeAttributeRepository(RepositoryImplBase<Domain.Model.Campaign.AdCreativeAttribute, int> repository)
            : base(repository)
        {
        }
    }
}
