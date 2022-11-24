using FluentNHibernate;
using FluentNHibernate.Mapping;
using ArabyAds.AdFalcon.Domain.Model.Account;
using ArabyAds.AdFalcon.Domain.Model.Account.SSP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArabyAds.AdFalcon.Persistence.Mappings.Account.SSP
{
    public class PartnerSiteMapping : ClassMap<PartnerSite>
    {
        public PartnerSiteMapping()
        {

            Schema("adfalcon_dsp");
            Table("dsp_sites");
            Id(x => x.ID).GeneratedBy.HiLo(MappingSettings.Schema + MappingSettings.HiLowTableName,
                                 MappingSettings._nextHi,
                                 MappingSettings._maxLo,
                                 "TableKey = 'PartnerSite'");

            References(p => p.Partner, "PartnerId").Cascade.None().Not.Nullable().LazyLoad();

            Map(p => p.Description, "Description");
            Map(p => p.SiteName, "DSPSiteName");
            Map(p => p.SiteID, "DSPSiteId");
            Map(p => p.IsDeleted, "IsDeleted");

        }
    }
}
