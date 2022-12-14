using FluentNHibernate;
using FluentNHibernate.Mapping;
using Noqoush.AdFalcon.Domain.Model.Account;
using Noqoush.AdFalcon.Domain.Model.Account.SSP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Noqoush.AdFalcon.Persistence.Mappings.Account.SSP
{
    public class DealCampaignMappingMap : ClassMap<DealCampaignMapping>
    {
        public DealCampaignMappingMap()
        {
            Schema("netcore31_adfalcon_dsp");
            Table("dsp_deals");
            Id(x => x.ID).GeneratedBy.HiLo(MappingSettings.Schema + MappingSettings.HiLowTableName,
                                 MappingSettings._nextHi,
                                 MappingSettings._maxLo,
                                 "TableKey = 'DealCampaignMapping'");

            References(p => p.Partner, "PartnerId").Cascade.None().Not.Nullable().LazyLoad();

            Map(p => p.IsDeleted);
            Map(p => p.DealId, "DealId");

            //  Map(p => p.AdFalconCampaignId, "AdFalconCampaignId");
            //Map(p => p.FloorPriceConfigType, "TypeId");
            References(p => p.Campaign, "AdFalconCampaignId").Cascade.None().Not.Nullable().LazyLoad();
        }
    }
}
