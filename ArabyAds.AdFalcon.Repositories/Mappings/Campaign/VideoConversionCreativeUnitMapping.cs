using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
namespace ArabyAds.AdFalcon.Persistence.Mappings.Campaign
{


   


    public class VideoConversionCreativeUnitMapping : ClassMap<ArabyAds.AdFalcon.Domain.Model.Campaign.VideoConversionCreativeUnit>
    {
        public VideoConversionCreativeUnitMapping()
        {
            Table("video_conversion_creativeunit");
            Id(x => x.ID).GeneratedBy.HiLo(MappingSettings.HiLowTableName, MappingSettings._nextHi, MappingSettings._maxLo, "TableKey = 'VideoConversionCreativeUnit'");

            Map(X=>X.BitRate);


            Map(X => X.AudioBitRate);
            Map(X => X.VideoFrameRate);

            Map(X => X.Code);
            References(x => x.CreativeUnit, "CreativeUnitId");
            Cache.Transactional().ReadWrite().IncludeAll();
        }
    }
}
