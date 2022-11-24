using FluentNHibernate.Mapping;

namespace Noqoush.AdFalcon.Persistence.Mappings.Campaign.AdCreative
{
    public class TileImageDocumentMapping : ClassMap<Noqoush.AdFalcon.Domain.Model.Campaign.TileImageDocument>
    {
        public TileImageDocumentMapping()
        {
            Table("tileimagesizedocuments");
            Id(x => x.ID).GeneratedBy.HiLo(MappingSettings.HiLowTableName, MappingSettings._nextHi, MappingSettings._maxLo, "TableKey = 'TileImageDocument'");
            References(x => x.TileImage, "TileImageId");
            References(x => x.TileImageSize, "TileImageSizeId");
            References(x => x.Document, "DocumentId");
            Map(x => x.URL);
        }
    }
}
