using FluentNHibernate.Mapping;
using ArabyAds.AdFalcon.Domain.Model.Core;
using ArabyAds.AdFalcon.Domain.Model.Core.Video;

namespace ArabyAds.AdFalcon.Persistence.Mappings.Core.Video
{
    public class PlaybackMethodsMapping : ClassMap<PlaybackMethods>
    {
        public PlaybackMethodsMapping()
        {
            Table("video_playback_methods");
            Id(x => x.ID).GeneratedBy.HiLo(MappingSettings.HiLowTableName,
                                           MappingSettings._nextHi,
                                           MappingSettings._maxLo,
                                           "TableKey = 'PlayBackMethod'");
            Map(x => x.Code);
   
            References(x => x.Name, "NameId").Cascade.All();
            Cache.Transactional().ReadWrite().IncludeAll();
        }
    }
}