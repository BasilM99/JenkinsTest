using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using Noqoush.AdFalcon.Domain.Common.Model.Campaign;
using Noqoush.AdFalcon.Domain.Common.Model.Core;
using Noqoush.AdFalcon.Domain.Model.Campaign;
using Noqoush.AdFalcon.Domain.Model.Core;
using Noqoush.AdFalcon.EventDTOs;

namespace Noqoush.AdFalcon.Persistence.Mappings.Campaign
{
    //campaigns
    public class CampaignMapping : ClassMap<Noqoush.AdFalcon.Domain.Model.Campaign.Campaign>
    {
        public CampaignMapping()
        {
            Table("campaigns");
            Id(x => x.ID).GeneratedBy.HiLo(MappingSettings.HiLowTableName, MappingSettings._nextHi, MappingSettings._maxLo, "TableKey = 'Campaign'");
            Map(x => x.Name);
            References(x => x.User, "UserId");
            Map(p => p.UniqueId);
            Map(x => x.StartDate);
            Map(x => x.CPMValue);
            Map(x => x.LogAdMarkup);
            Map(x => x.PriceMode, "PriceMode").CustomType(typeof(PriceMode)); ;
            //Map(x => x.Advertiser);
            Map(x => x.EndDate).Nullable();
            Map(x => x.StartTime).Nullable();
            Map(x => x.EndTime).Nullable();
            Map(x => x.IsClientLocked).Nullable();
            Map(x => x.CostModelWrapper, "CostModelWrapperId").Nullable().CustomType<CostModelWrapperEnum?>();
            Map(x => x.CreationDate);
            Map(x => x.Budget);
            Map(x => x.DailyBudget);
            Map(x => x.Note);
           // Map(x => x.LifeTime).CustomType(typeof(CampaignLifeTime));
            
            Map(x => x.ModifiedOn).ReadOnly();
            Map(x => x.Pacing, "Pacing").CustomType(typeof(PacingPolicies));
            //Map(x => x.NameLower);

            Map(x => x.IsDeleted);
            References(x => x.Advertiser, "AdvertiserId").Not.LazyLoad().Cascade.None().Nullable();
            Map(x => x.FolderURL);
            Map(x => x.CampaignType, "TypeId").CustomType(typeof(CampaignType));
            Map(x => x.IsRuntime, "isRuntime");
            References(x => x.Account, "AccountId");

            References(x => x.AdvertiserAccount, "AssociationAdvId");
            Map(x => x.TrackConversions, "TrackConversions");
            
            //References(x => x.Status, "StatusId");
            HasMany(d => d.AdGroups).KeyColumn("CampaignId").Cascade.All();
            HasMany(d => d.CampaignAssignedAppsites).KeyColumn("CampaignId").Cascade.All().Where("AdGroupId IS NULL");//.Where(x => !x.IsDeleted).Cascade.All();
            // HasMany(d => d.CampaignBidConfigs).KeyColumn("CampaignId").Cascade.All(); //.Where(x => !x.IsDeleted).Cascade.All();
            //HasOne(p => p.Performance).PropertyRef(p => p.Campaign).Cascade.None();
            Component(x => x.Discount, m =>
            {
                m.Map(x => x.Value, "Discount").Nullable();
                m.Map(x => x.FromDate, "DiscountFromDate").Nullable();
                m.Map(x => x.ToDate, "DiscountToDate").Nullable();
                m.Map(x => x.Type, "DiscountType").CustomType<DiscountType>().Nullable();
            });
            //Map(x => x.DomainURL).Nullable();
            References(x => x.Keyword, "KeywordId").Nullable();
            HasOne(d => d.CampaignServerSetting).LazyLoad().ForeignKey("Id").Cascade.SaveUpdate().Constrained();
           // Map(X => X.AgencyCommission, "AgencyCommissionMode");

        }
    }
}
