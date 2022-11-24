using System;
using System.Collections.Generic;
using System.Text;
using FluentNHibernate.Mapping;
using System.Threading.Tasks;
using ArabyAds.AdFalcon.Domain.Common.Model.Campaign;

namespace ArabyAds.AdFalcon.Persistence.Mappings.Campaign
{


    public class AdGroupBidModifierMapping : ClassMap<ArabyAds.AdFalcon.Domain.Model.Campaign.AdGroupBidModifier>
    {
        public AdGroupBidModifierMapping()
        {
            Table("adgroup_bid_modifiers");
            Id(x => x.ID, "Id").GeneratedBy.HiLo(MappingSettings.HiLowTableName, MappingSettings._nextHi,
                                                MappingSettings._maxLo, "TableKey = 'AdGroupBidModifier'");

            Where("AdGroupId > 0 ");
            References(x => x.Campaign, "CampaignId");


            References(x => x.AdGroup, "AdGroupId");
            Map(x => x.DimentionType, "DimensionType").CustomType<DimentionType>(); ;
            Map(x => x.DimensionValue, "DimensionValue");
            Map(x => x.Multiplier);

            Map(x => x.IsDeleted);

        }
    }
}
