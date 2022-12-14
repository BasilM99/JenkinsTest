using FluentNHibernate;
using FluentNHibernate.Mapping;
using Noqoush.AdFalcon.Domain.Common.Model.Account;
using Noqoush.AdFalcon.Domain.Model.Account;
using Noqoush.AdFalcon.Domain.Model.Campaign;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Noqoush.AdFalcon.Persistence.Mappings.Campaign
{
    public class CampaignServerSettingMapping : ClassMap<CampaignServerSetting>
    {
        public CampaignServerSettingMapping()
        {
            Table("campaign_adserver_settings");

            Id(Reveal.Member<CampaignServerSetting>("CampaignId")).Column("CampaignId").GeneratedBy.Foreign("Campaign");
            HasOne(Reveal.Member<CampaignServerSetting, Domain.Model.Campaign.Campaign>("Campaign")).Constrained();


            Map(x => x.AdRequestCacheLifeTime, "AdRequestCachedDataLifeTime");
            HasMany(p => p.FrequencyCappingList).KeyColumn("CampaignId").Cascade.AllDeleteOrphan().Inverse();
            Map(X => X.AgencyCommission, "AgencyCommissionModel").CustomType(typeof(AgencyCommission)).Nullable(); ;
            Map(X => X.AgencyCommissionValue, "AgencyCommissionModelValue");
        }
    }
}
