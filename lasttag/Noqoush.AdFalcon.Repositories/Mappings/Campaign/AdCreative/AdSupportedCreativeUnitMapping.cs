using FluentNHibernate.Mapping;
using Noqoush.AdFalcon.Domain.Common.Model.Campaign;
using Noqoush.AdFalcon.Domain.Common.Model.Core;
using Noqoush.AdFalcon.Domain.Model.Campaign;
using Noqoush.AdFalcon.Domain.Model.Core;

namespace Noqoush.AdFalcon.Persistence.Mappings.Campaign.AdCreative
{
    public class AdSupportedCreativeUnitMapping : ClassMap<Noqoush.AdFalcon.Domain.Model.Campaign.AdSupportedCreativeUnit>
    {
        public AdSupportedCreativeUnitMapping()
        {
            Table("ad_supported_creativeunits");
            Id(x => x.ID).GeneratedBy.HiLo(MappingSettings.HiLowTableName, MappingSettings._nextHi, MappingSettings._maxLo, "TableKey = 'AdSupportedCreativeUnit'");
            References(x => x.AdType, "AdTypeId");
            References(x => x.CreativeUnit, "CreativeUnitId");
            References(x => x.OrientationReplacement, "OrientationReplacementId");
            Map(x => x.AdSubType, "AdSubTypeId").CustomType(typeof(AdSubTypes));
            Map(x => x.EnvironmentType, "EnvironmentTypeId").CustomType(typeof(EnvironmentType));
            Map(x => x.RequiredType).CustomType<RequiredType>();
        }
    }
}
