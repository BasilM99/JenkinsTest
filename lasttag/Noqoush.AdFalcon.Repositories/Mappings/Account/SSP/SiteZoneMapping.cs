using System;
using System.Collections.Generic;
using FluentNHibernate;
using FluentNHibernate.Mapping;
using Noqoush.AdFalcon.Domain.Model.Account;
using Noqoush.AdFalcon.Domain.Model.Account.SSP;

using System.Linq;
using System.Text;

namespace Noqoush.AdFalcon.Persistence.Mappings.Account.SSP
{
    public class SiteZoneMapping : ClassMap<SiteZone>
    {
        public SiteZoneMapping()
        {

            Schema("netcore31_adfalcon_dsp");
            Table("dsp_site_zones");
            Id(x => x.ID).GeneratedBy.HiLo(MappingSettings.Schema + MappingSettings.HiLowTableName,
                                 MappingSettings._nextHi,
                                 MappingSettings._maxLo,
                                 "TableKey = 'SiteZone'");

            References(p => p.Site, "SiteId").Cascade.None().Not.Nullable().LazyLoad();

            Map(p => p.Description, "Description").Nullable();
            Map(p => p.ZoneID, "DSPZoneId");
            Map(p => p.ZoneName, "DSPZoneName").Nullable();
            Map(p => p.IsDeleted, "IsDeleted");

        }
    }
}
