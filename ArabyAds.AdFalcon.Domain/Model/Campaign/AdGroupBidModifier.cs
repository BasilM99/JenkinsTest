using System;
using System.Collections.Generic;
using System.Text;

namespace ArabyAds.AdFalcon.Domain.Model.Campaign
{
    public class AdGroupBidModifier: CampaignBidModifier
    {

        public virtual AdGroup AdGroup { get; set; }

        public virtual AdGroupBidModifier CloneAdGroupBidModifier()
        {
            var cloneObj = new AdGroupBidModifier()
            {
                Campaign = this.Campaign,
                AdGroup = this.AdGroup,
                IsDeleted = this.IsDeleted,
                DimentionType = this.DimentionType,

                DimensionValue = this.DimensionValue,
                Multiplier = this.Multiplier,

            };
            return cloneObj;

        }

    }
}
