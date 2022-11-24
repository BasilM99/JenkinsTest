using FluentNHibernate;
using FluentNHibernate.Automapping;
using FluentNHibernate.Mapping;
using ArabyAds.AdFalcon.Domain.Model.Campaign;
using ArabyAds.AdFalcon.Domain.Model.Core;


namespace ArabyAds.AdFalcon.Persistence.Mappings.Campaign.AdCreative
{
  
    public class AdCreativeUnitAttributeMapping : ClassMap<ArabyAds.AdFalcon.Domain.Model.Campaign.AdCreativeUnitAttributeMapping>
    {
        public AdCreativeUnitAttributeMapping()
        {


            Table("ad_creative_unit_attributes");
            Id(x => x.ID).GeneratedBy.HiLo(MappingSettings.HiLowTableName, MappingSettings._nextHi,
                                      MappingSettings._maxLo, "TableKey = 'AdCreativeUnitAttributeMapping'");

            References(p => p.Attribute, "CreativeAttributeId").Cascade.None();
            //Map(p => p.IsDeleted);
            References(p => p.AdCreativeUnit, "AdCreativeUnitId").LazyLoad().Cascade.None();
        }
    }
}
