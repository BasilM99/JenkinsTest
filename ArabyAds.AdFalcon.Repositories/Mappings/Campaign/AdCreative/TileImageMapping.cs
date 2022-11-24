using FluentNHibernate.Mapping;

namespace ArabyAds.AdFalcon.Persistence.Mappings.Campaign.AdCreative
{
    public class TileImageMapping : ClassMap<ArabyAds.AdFalcon.Domain.Model.Campaign.TileImage>
    {
        public TileImageMapping()
        {
            Table("tileimages");
            Id(x => x.ID).GeneratedBy.HiLo(MappingSettings.HiLowTableName, MappingSettings._nextHi,
                                           MappingSettings._maxLo, "TableKey = 'TileImage'");
            References(x => x.Name, "NameID");
            HasMany(x => x.Images).KeyColumn("TileImageId").Cascade.All().BatchSize(100).Cache.Transactional().ReadWrite();
            Map(x => x.IsCustom);
            Map(x => x.IsClickAction);
            Cache.Transactional().ReadWrite().IncludeAll();
        }
    }
}
