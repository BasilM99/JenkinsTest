using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Noqoush.AdFalcon.Persistence.Mappings.Campaign
{


    public class VideoMediaFileMapping : ClassMap<Noqoush.AdFalcon.Domain.Model.Campaign.VideoMediaFile>
    {
        public VideoMediaFileMapping()
        {
            Table("video_ad_media_files");
            Id(x => x.ID).GeneratedBy.HiLo(MappingSettings.HiLowTableName, MappingSettings._nextHi,
                                         MappingSettings._maxLo, "TableKey = 'VideoMediaFile'");
            Map(p => p.BitRate);
            References(p => p.OriginalCreativeUnit, "CreativeUnitId");
            References(x => x.Document, "DocumentId");
          //  References(x => x.VideoAd, "VideoAdId");
            References(x => x.VideoType, "MIMETypeId");
            References(x => x.DeliveryMethod, "DeliveryMethodId");
            References(x => x.AdCreativeUnit, "VideoAdCreativeUnitId");

            Map(x => x.URL, "Url");
        }
    }
}
