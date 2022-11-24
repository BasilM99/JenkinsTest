using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
namespace ArabyAds.AdFalcon.Persistence.Mappings.Campaign
{
    public class TileImageSizeMapping : ClassMap<ArabyAds.AdFalcon.Domain.Model.Campaign.TileImageSize>
    {
        public TileImageSizeMapping()
        {
            Table("tileimagesizes");
            Id(x => x.ID).GeneratedBy.HiLo(MappingSettings.HiLowTableName, MappingSettings._nextHi, MappingSettings._maxLo, "TableKey = 'TileImageSize'");
            References(x => x.Name, "NameID");
            Map(x => x.Width);
            Map(x => x.Height);
            Map(x => x.IsDeleted);
            References(x => x.DeviceType, "DeviceTypeId");
            Map(x => x.IsActionTile);
            References(x => x.TitleSize, "TitleSizeId").Nullable();
            HasMany(x => x.Formats).KeyColumn("TileImageSizeId").Cascade.All().BatchSize(100).Cache.Transactional().ReadWrite();
            Cache.Transactional().ReadWrite().IncludeAll();
            BatchSize(100);
        }
    }
}
