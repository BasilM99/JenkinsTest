using FluentNHibernate.Mapping;
using Noqoush.AdFalcon.Domain.Model.Core;

namespace Noqoush.AdFalcon.Persistence.Mappings.Core
{
    public class TileImageUnitFormatMapping : ClassMap<TileImageFormat>
    {
        public TileImageUnitFormatMapping()
        {
            Table("tileimageformats");

            Id(x => x.ID).GeneratedBy.HiLo(MappingSettings.HiLowTableName,
                                           MappingSettings._nextHi,
                                           MappingSettings._maxLo,
                                           "TableKey = 'TileImageFormat'");
            Map(x => x.Format);
            Map(x => x.MaxSize);
            Map(x => x.IsDeleted);
            References(x => x.TileImageSize, "TileImageSizeId");
        }
    }
}