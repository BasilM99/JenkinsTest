using FluentNHibernate.Mapping;
using ArabyAds.AdFalcon.Domain.Model.Campaign;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArabyAds.AdFalcon.Domain.Model.Core;
using ArabyAds.AdFalcon.Domain.Model.Campaign.Targeting;

namespace ArabyAds.AdFalcon.Persistence.Mappings.Core
{
    public class AdRequestTypePlatformVersionMapping : ClassMap<AdRequestTypePlatformVersion>
    {
        public AdRequestTypePlatformVersionMapping()
        {
            Table("adrequest_types_platforms_mapping");
            Id(x => x.ID).GeneratedBy.HiLo(MappingSettings.HiLowTableName, MappingSettings._nextHi,
                                      MappingSettings._maxLo, "TableKey = 'AdRequestTypePlatformVersion'");
            Map(p => p.Version);

            References(p => p.AdRequestPlatform, "AdRequestPlatformId").Cascade.None();
            References(p => p.AdRequestType, "AdRequestTypeId").Cascade.None();
        }
    }
}
