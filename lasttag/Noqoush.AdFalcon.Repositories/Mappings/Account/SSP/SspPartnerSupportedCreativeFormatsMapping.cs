using FluentNHibernate;
using FluentNHibernate.Mapping;
using Noqoush.AdFalcon.Domain.Common.Model.Campaign;
using Noqoush.AdFalcon.Domain.Model.Account;
using Noqoush.AdFalcon.Domain.Model.Account.SSP;
using Noqoush.AdFalcon.Domain.Model.Campaign;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Noqoush.AdFalcon.Persistence.Mappings.Account.SSP
{
    public class SSPPartnerSupportedCreativeFormatsMapping : ClassMap<SSPPartnerSupportedCreativeFormats>
    {
        public SSPPartnerSupportedCreativeFormatsMapping()
        {

            Table("ssp_partner_supported_creative_formats");
            Id(x => x.ID).GeneratedBy.HiLo(MappingSettings.Schema + MappingSettings.HiLowTableName,
                                 MappingSettings._nextHi,
                                 MappingSettings._maxLo,
                                 "TableKey = 'SSPPartnerSupportedCreativeFormats'");

            References(p => p.Partner, "PartnerId").Cascade.None().Not.Nullable().LazyLoad();
            References(p => p.CreativeFormat, "CreativeFormatId").Cascade.None().Not.Nullable().LazyLoad();
            Map(x => x.EnvironmentType, "EnvironmentType").CustomType<EnvironmentType>();
            //Map(p => p.IsDeleted, "IsDeleted");

        }
    }
}
