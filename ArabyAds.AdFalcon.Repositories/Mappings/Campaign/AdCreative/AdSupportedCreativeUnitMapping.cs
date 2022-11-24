using FluentNHibernate.Mapping;
using ArabyAds.AdFalcon.Domain.Common.Model.Campaign;
using ArabyAds.AdFalcon.Domain.Common.Model.Core;
using ArabyAds.AdFalcon.Domain.Model.Campaign;
using ArabyAds.AdFalcon.Domain.Model.Core;

namespace ArabyAds.AdFalcon.Persistence.Mappings.Campaign.AdCreative
{
    public class AdSupportedCreativeUnitMapping : ClassMap<ArabyAds.AdFalcon.Domain.Model.Campaign.AdSupportedCreativeUnit>
    {
        public AdSupportedCreativeUnitMapping()
        {
            Table("ad_supported_creativeunits");
            Id(x => x.ID).GeneratedBy.HiLo(MappingSettings.HiLowTableName, MappingSettings._nextHi, MappingSettings._maxLo, "TableKey = 'AdSupportedCreativeUnit'");
            References(x => x.AdType, "AdTypeId").Not.LazyLoad().Fetch.Select();
            References(x => x.CreativeUnit, "CreativeUnitId").Not.LazyLoad().Fetch.Select();
            References(x => x.OrientationReplacement, "OrientationReplacementId");
            Map(x => x.AdSubType, "AdSubTypeId").CustomType(typeof(AdSubTypes));
            Map(x => x.EnvironmentType, "EnvironmentTypeId").CustomType(typeof(EnvironmentType));
            Map(x => x.RequiredType).CustomType<RequiredType>();
            Cache.Transactional().ReadWrite().IncludeAll();

        }
    }
}
