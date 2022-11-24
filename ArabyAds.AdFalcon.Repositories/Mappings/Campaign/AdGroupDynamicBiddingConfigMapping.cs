using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using ArabyAds.AdFalcon.Domain.Model.Campaign;
using ArabyAds.AdFalcon.Domain.Model.Core;
using ArabyAds.AdFalcon.Domain.Model.Account.PMP;
using ArabyAds.AdFalcon.EventDTOs;
using FluentNHibernate;
using ArabyAds.AdFalcon.Domain.Common.Model.Campaign;

namespace ArabyAds.AdFalcon.Persistence.Mappings.Campaign
{
  

    public class AdGroupDynamicBiddingConfigMapping : ClassMap<AdGroupDynamicBiddingConfig>
    {
        public AdGroupDynamicBiddingConfigMapping()
        {

            Table("adgroup_dynamic_bidding_config");
            // ALi must me changed to TableKey ="AdGroupCostElement"
            Id(x => x.ID, "Id").GeneratedBy.HiLo(MappingSettings.HiLowTableName, MappingSettings._nextHi, MappingSettings._maxLo, "TableKey = 'AdGroupDynamicBiddingConfig'");

            Map(x => x.KeepBiddingAtMinimum);
            Map(x => x.BidStep);
            Map(x => x.MinBidPrice);
            Map(x => x.MaxBidPrice);
            Map(x => x.DefaultBidPrice);


            Map(x => x.BidOptimizationValue);
            Map(x => x.Type, "BidOptimizationType").CustomType(typeof(BidOptimizationType));
            References(r => r.AdGroup, "AdGroupId");
            //HasOne(Reveal.Member<AdGroupDynamicBiddingConfig, AdGroup>("AdGroup")).Constrained().ForeignKey("AdGroupId").Constrained();

        }

    }
}
