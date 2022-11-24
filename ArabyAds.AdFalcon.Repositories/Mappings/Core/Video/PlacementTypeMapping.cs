using FluentNHibernate.Mapping;
using ArabyAds.AdFalcon.Domain.Model.Core;
using ArabyAds.AdFalcon.Domain.Model.Core.Video;

namespace ArabyAds.AdFalcon.Persistence.Mappings.Core.Video
{
    public class PlacementTypeMapping : ClassMap<PlacementType>
    {
        public PlacementTypeMapping()
        {
            Table("video_placement_types");
            Id(x => x.ID).GeneratedBy.HiLo(MappingSettings.HiLowTableName,
                                           MappingSettings._nextHi,
                                           MappingSettings._maxLo,
                                           "TableKey = 'VideoPlacementType'");
            Map(x => x.Code);
 
            References(x => x.Name, "NameId").Cascade.All();
            Cache.Transactional().ReadWrite().IncludeAll();
        }
    }
}