using FluentNHibernate;
using FluentNHibernate.Mapping;
using ArabyAds.AdFalcon.Domain.Common.Model.Account.SSP;
using ArabyAds.AdFalcon.Domain.Model.Account;
using ArabyAds.AdFalcon.Domain.Model.Account.SSP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArabyAds.AdFalcon.Persistence.Mappings.Account.SSP
{
    public class FloorPriceMapping : ClassMap<FloorPrice>
    {
        public FloorPriceMapping()
        {
            Schema("adfalcon_dsp");
            Table("dsp_site_floor_price_config");
            Id(x => x.ID).GeneratedBy.HiLo(MappingSettings.Schema + MappingSettings.HiLowTableName,
                                 MappingSettings._nextHi,
                                 MappingSettings._maxLo,
                                 "TableKey = 'FloorPrice'");

            References(p => p.Site, "SiteId").Cascade.None().Not.Nullable().LazyLoad();
            References(p => p.Zone, "ZoneId").Cascade.None().Nullable().LazyLoad();
            Map(p => p.IsDeleted);
            Map(p => p.Price, "Value");
            Map(p => p.ConfigType, "TypeId").CustomType<FloorPriceConfigType>().Not.Nullable();
            Map(p => p.TargetingId, "TargetingId").Nullable();
            //Map(p => p.FloorPriceConfigType, "TypeId");

        }
    }
}
