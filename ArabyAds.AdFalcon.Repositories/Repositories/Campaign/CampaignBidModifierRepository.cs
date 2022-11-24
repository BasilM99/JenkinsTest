using System;
using System.Collections.Generic;
using System.Text;
using ArabyAds.AdFalcon.Domain.Model.AppSite;
using ArabyAds.AdFalcon.Domain.Model.Campaign;
using ArabyAds.AdFalcon.Domain.Repositories.Campaign;
using ArabyAds.Framework.Persistence;

namespace ArabyAds.AdFalcon.Persistence.Repositories.Campaign
{
    public class CampaignBidModifierRepository : RepositoryBase<Domain.Model.Campaign.CampaignBidModifier, int>, ICampaignBidModifierRepository
    {
        public CampaignBidModifierRepository(RepositoryImplBase<CampaignBidModifier, int> repository)
         : base(repository)
        {
        }
    }
}
