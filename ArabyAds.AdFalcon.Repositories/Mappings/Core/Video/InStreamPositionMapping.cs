using FluentNHibernate.Mapping;
using ArabyAds.AdFalcon.Domain.Model.Core;
using ArabyAds.AdFalcon.Domain.Model.Core.Video;

namespace ArabyAds.AdFalcon.Persistence.Mappings.Core.Video
{
    public class InStreamPositionMapping : ClassMap<InStreamPosition>
    {
        public InStreamPositionMapping()
        {
            Table("video_instream_positions");
            Id(x => x.ID).GeneratedBy.HiLo(MappingSettings.HiLowTableName,
                                           MappingSettings._nextHi,
                                           MappingSettings._maxLo,
                                           "TableKey = 'InStreamPosition'");
            Map(x => x.Code);
           
            References(x => x.Name, "NameId").Cascade.All();
            Cache.Transactional().ReadWrite().IncludeAll();
        }
    }
}