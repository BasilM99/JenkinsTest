using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using Noqoush.AdFalcon.Domain.Model.Campaign;
using Noqoush.AdFalcon.Domain.Model.Core;
using Noqoush.AdFalcon.Domain.Model.Account.PMP;
using Noqoush.AdFalcon.EventDTOs;
using Noqoush.AdFalcon.Domain.Common.Model.Campaign;

namespace Noqoush.AdFalcon.Persistence.Mappings.Campaign
{
    public class AdGroupMapping : ClassMap<Noqoush.AdFalcon.Domain.Model.Campaign.AdGroup>
    {
        public AdGroupMapping()
        {
            Table("adgroups");
            Id(x => x.ID).GeneratedBy.HiLo(MappingSettings.HiLowTableName, MappingSettings._nextHi,
                                           MappingSettings._maxLo, "TableKey = 'AdGroup'");
            References(x => x.Campaign, "CampaignId");
            Map(x => x.Bid);
            Map(x => x.MinimumUnitPrice);
            
                            Map(x => x.CountingTypeAttribuation).CustomType(typeof(CountingTypeAttribuation)); ;

            Map(x => x.AudianceDiscountPrice);
            Map(x => x.Name);
            Map(x => x.IsDeleted);
            Map(x => x.DisableProxyTraffic);
            Map(x => x.AllowOpenAuction);
            Map(p => p.UniqueId);
            Map(p => p.LogAdMarkup);
            Map(x => x.AdPosition);
            Map(x=>x.RunAllExchanges);
            Map(x => x.ViewabilityVendorId);
            

            Map(x => x.DataBid,"DataPrice");
            Map(p => p.MaxDataBid,"MaxDataPrice");

            Map(x => x.ConversionSetting, "ConversionSetting").CustomType(typeof(ConversionSetting));
            Map(x => x.ConversionType, "AttributionType").CustomType(typeof(ConversionType));
            Map(p => p.ViewAttribuation, "ImpressionAttributionWindow");
            Map(x => x.ClickAttribuation, "ClickAttributionWindow");
            Map(p => p.CountingAttribuation, "CountingAttribuation");

       

        //Map(p => p.MinimumUnitPrice);
        // Map(x => x.NameLower);
            Map(x => x.BiddingStrategy, "BiddingStrategy").CustomType(typeof(BiddingStrategy));

            Map(x => x.ConnectionType, "ConnectionType").CustomType(typeof(TargetingConnectionType)).Nullable();
            Map(x => x.Pacing, "Pacing").CustomType(typeof(PacingPolicies));
            Map(x => x.DailyBudget).Nullable();
            Map(x => x.Budget).Nullable();
            Map(x => x.CPMValue);
            Map(x => x.CreationDate);
            Map(x => x.TrackInstalls);
            Map(x => x.OpenInExternalBrowser);
            Map(x => x.IsDefaultPrerequisitesSaved);
            Map(x => x.IsCostModelChanged);
            Map(x=>x.IgnoreDailyBudget, "IgnoreDailyBudget");
            HasOne(p => p.Objective).PropertyRef(p => p.AdGroup).Cascade.All().Constrained();
            HasMany(d => d.Targetings).KeyColumn("AdGroupId").Cascade.AllDeleteOrphan();
            HasMany(d => d.Ads).KeyColumn("AdGroupId").Cascade.All();
            HasMany(d => d.CostElements).KeyColumn("AdGroupId").Where(M => M.Type == 1).Cascade.AllDeleteOrphan().Inverse();

            //HasMany(d => d.AdGroupDynamicBiddingConfigs).KeyColumn("AdGroupId").Cascade.AllDeleteOrphan().Inverse();

            HasOne(x => x.AdGroupDynamicBiddingConfig).PropertyRef(x=>x.AdGroup).Cascade.All();

            HasMany(d => d.Fees).KeyColumn("AdGroupId").Where(M => M.Type == 2).Cascade.AllDeleteOrphan().Inverse();
            // HasMany(d => d.CostElements).KeyColumn("AdGroupId").Cascade.AllDeleteOrphan();
            Map(x => x.ModifiedOn).ReadOnly();
            HasOne(x => x.HouseAd).ForeignKey("Id").Cascade.All();
            HasMany(x => x.TrackingEvents).KeyColumn("AdGroupId").Cascade.AllDeleteOrphan().Where("IsTracking=1");
            References(x => x.CostModelWrapper,"CostModelWrapperId");
            HasMany(d => d.CampaignBidConfigs).KeyColumn("AdGroupId").Cascade.All(); //.Where(x => !x.IsDeleted).Cascade.All();
            HasMany(d => d.AdGroupInventorySources).KeyColumn("AdGroupId").Cascade.All().Where("AdGroupId>0");

            HasMany(x => x.ConversionEvents).KeyColumn("AdGroupId").Cascade.AllDeleteOrphan().Where("IsConversion=1");
           // HasMany(x => x.AdGroupEvents).KeyColumn("AdGroupId").Cascade.AllDeleteOrphan();
        }

    }
}
