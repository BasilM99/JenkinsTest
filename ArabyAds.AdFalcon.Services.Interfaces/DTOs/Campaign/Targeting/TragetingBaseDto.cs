using System;
using System.Collections.Generic;
using System.Linq;
using ProtoBuf;
using System.Text;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Core;

namespace ArabyAds.AdFalcon.Services.Interfaces.DTOs.Campaign.Targeting
{
    [ProtoContract]
    [ProtoInclude(100,typeof(DeviceTargetingDto))]
    [ProtoInclude(101,typeof(KeywordTargetingDto))]
    [ProtoInclude(102,typeof(GeographicTargetingDto))]
    [ProtoInclude(103,typeof(OperatorTargetingDto))]
    [ProtoInclude(104,typeof(IPTargetingDto))]
    [ProtoInclude(105,typeof(DemographicTargetingDto))]
    [ProtoInclude(106,typeof(URLTargetingDto))]
    [ProtoInclude(107,typeof(GeoFencingTargetingDto))]
    [ProtoInclude(108,typeof(AdRequestTargetingDto))]
    [ProtoInclude(109,typeof(ImpressionMetricTargetingDto))]
    [ProtoInclude(110,typeof(LanguageTargetingDto))]
    [ProtoInclude(111,typeof(VideoTargetingDto))]

    
    public class TargetingBaseDto
    {
       [ProtoMember(1)]
        public int ID { get;  set; }
       [ProtoMember(2)]
        public TargetingTypeDto Type { get; set; }
    }
}
