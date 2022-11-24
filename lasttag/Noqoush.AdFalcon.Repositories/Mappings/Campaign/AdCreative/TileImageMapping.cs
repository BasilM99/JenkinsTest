using FluentNHibernate.Mapping;

namespace Noqoush.AdFalcon.Persistence.Mappings.Campaign.AdCreative
{
    public class TileImageMapping : ClassMap<Noqoush.AdFalcon.Domain.Model.Campaign.TileImage>
    {
        public TileImageMapping()
        {
            Table("tileimages");
            Id(x => x.ID).GeneratedBy.HiLo(MappingSettings.HiLowTableName, MappingSettings._nextHi,
                                           MappingSettings._maxLo, "TableKey = 'TileImage'");
            References(x => x.Name, "NameID");
            HasMany(x => x.Images).KeyColumn("TileImageId").Cascade.All();
            Map(x => x.IsCustom);
            Map(x => x.IsClickAction);
            Cache.Transactional().ReadWrite().IncludeAll();
        }
    }
}
