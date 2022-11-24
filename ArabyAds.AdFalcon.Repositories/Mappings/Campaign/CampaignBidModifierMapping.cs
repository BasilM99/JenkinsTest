using System;
using System.Collections.Generic;
using System.Text;
using FluentNHibernate.Mapping;
using System.Threading.Tasks;
using ArabyAds.AdFalcon.Domain.Common.Model.Campaign;

namespace ArabyAds.AdFalcon.Persistence.Mappings.Campaign
{
   
    public class CampaignBidModifierMapping : ClassMap<ArabyAds.AdFalcon.Domain.Model.Campaign.CampaignBidModifier>
    {
        public CampaignBidModifierMapping()
        {
            Table("adgroup_bid_modifiers");
            Id(x => x.ID, "Id").GeneratedBy.HiLo(MappingSettings.HiLowTableName, MappingSettings._nextHi,
                                                MappingSettings._maxLo, "TableKey = 'AdGroupBidModifier'");

            Where("AdGroupId IS NULL");
            References(x => x.Campaign, "CampaignId");
            Map(x => x.DimentionType, "DimensionType").CustomType<DimentionType>(); ;
            Map(x => x.DimensionValue, "DimensionValue");
            Map(x => x.Multiplier);
            
            Map(x => x.IsDeleted);
            //Map(x => x.Include);
            //References(x => x.SubAppsite, "SubAppSiteId").LazyLoad();
        }
    }
}
