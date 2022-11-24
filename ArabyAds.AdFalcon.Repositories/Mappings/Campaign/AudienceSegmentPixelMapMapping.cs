using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using ArabyAds.AdFalcon.Domain.Model.Campaign.Targeting.Device;
using ArabyAds.AdFalcon.Domain.Model.Campaign.Targeting;
using ArabyAds.AdFalcon.Domain.Model.Campaign;

namespace ArabyAds.AdFalcon.Persistence.Mappings.Campaign
{
   

    public class AudienceSegmentPixelMapMapping : ClassMap<AudienceSegmentPixelMap>
    {
        public AudienceSegmentPixelMapMapping()
        {
            Table("pixel_audience_segments");
            Id(x => x.ID).GeneratedBy.HiLo(MappingSettings.HiLowTableName, MappingSettings._nextHi,
                                           MappingSettings._maxLo, "TableKey = 'AudienceSegmentPixelMap'");
            References(x => x.Pixel, "PixelId");
            References(x => x.AudienceSegment, "AudienceSegmentId");

        }
    }
}
