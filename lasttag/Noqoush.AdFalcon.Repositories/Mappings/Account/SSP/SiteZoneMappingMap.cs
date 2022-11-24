using System;
using System.Collections.Generic;
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
    public class SiteZoneMappingMapping : ClassMap<Noqoush.AdFalcon.Domain.Model.Account.SSP.SiteZoneMapping>
    {
        public SiteZoneMappingMapping()
        {
            Schema("netcore31_adfalcon_dsp");
            Table("dsp_sites_mapping");
            Id(x => x.ID).GeneratedBy.HiLo(MappingSettings.Schema + MappingSettings.HiLowTableName,
                                 MappingSettings._nextHi,
                                 MappingSettings._maxLo,
                                 "TableKey = 'SiteZoneMapping'");

            References(p => p.Site, "SiteId").Cascade.None().Not.Nullable().LazyLoad();

            References(p => p.Zone, "ZoneId").Cascade.None().Not.Nullable().LazyLoad();

            References(p => p.AppSite, "AdFalconAppSiteId").Cascade.None().Not.Nullable().LazyLoad();
            References(x => x.SubAppSite, "AdFalconSubAppSiteId").LazyLoad();


            References(p => p.DeviceType, "DeviceTypeId").Cascade.None().Nullable().LazyLoad();
            References(p => p.AdType, "AdTypeId").Cascade.None().Nullable().LazyLoad();

            Map(p => p.IsInterstitial, "IsInterstitial").Nullable();

            Map(p => p.AdFalconSubPublisherId).Nullable().Formula("(SELECT sub_appsites.SubPublisherId FROM sub_appsites where sub_appsites.Id=AdFalconSubAppSiteId)").Not.Insert().Not.Update();

            Map(p => p.IsDeleted, "IsDeleted");
        }
    }
}
