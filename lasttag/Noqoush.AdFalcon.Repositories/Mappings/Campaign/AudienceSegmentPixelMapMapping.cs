using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using Noqoush.AdFalcon.Domain.Model.Campaign.Targeting.Device;
using Noqoush.AdFalcon.Domain.Model.Campaign.Targeting;
using Noqoush.AdFalcon.Domain.Model.Campaign;

namespace Noqoush.AdFalcon.Persistence.Mappings.Campaign
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
