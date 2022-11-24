using FluentNHibernate.Mapping;

namespace Noqoush.AdFalcon.Persistence.Mappings.Campaign.AdCreative
{
    public class VideoDeliveryMethodMapping : ClassMap<Noqoush.AdFalcon.Domain.Model.Campaign.VideoDeliveryMethod>
    {
        public VideoDeliveryMethodMapping()
        {
            Table("video_delivery_methods");
            Id(x => x.ID).GeneratedBy.HiLo(MappingSettings.HiLowTableName, MappingSettings._nextHi, MappingSettings._maxLo, "TableKey = 'VideoDeliveryMethod'");
            Map(x => x.Code);
            References(x => x.Name, "NameID");
            Cache.Transactional().ReadWrite().IncludeAll();

        }
    }
}
