using FluentNHibernate.Mapping;
using ArabyAds.AdFalcon.Domain.Model.Core;
using ArabyAds.AdFalcon.Domain.Model.Core.Video;

namespace ArabyAds.AdFalcon.Persistence.Mappings.Core.Video
{
    public class SkippableAdsMapping : ClassMap<SkippableAds>
    {
        public SkippableAdsMapping()
        {
            Table("video_skippable_ad_options");
            Id(x => x.ID).GeneratedBy.HiLo(MappingSettings.HiLowTableName,
                                           MappingSettings._nextHi,
                                           MappingSettings._maxLo,
                                           "TableKey = 'SkippableAds'");
            Map(x => x.Code);
        
            References(x => x.Name, "NameId").Cascade.All();
            Cache.Transactional().ReadWrite().IncludeAll();
        }
    }
}