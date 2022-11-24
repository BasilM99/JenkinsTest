using System;
using System.Collections.Generic;
using System.Text;
using ArabyAds.AdFalcon.Domain.Model.AppSite;
using ArabyAds.AdFalcon.Domain.Model.Campaign;
using ArabyAds.AdFalcon.Domain.Repositories.Campaign;
using ArabyAds.Framework.Persistence;

namespace ArabyAds.AdFalcon.Persistence.Repositories.Campaign
{
    public class AdGroupBidModifierRepository : RepositoryBase<Domain.Model.Campaign.AdGroupBidModifier, int>, IAdGroupBidModifierRepository
    {
        public AdGroupBidModifierRepository(RepositoryImplBase<AdGroupBidModifier, int> repository)
          : base(repository)
        {
        }
    }
}
