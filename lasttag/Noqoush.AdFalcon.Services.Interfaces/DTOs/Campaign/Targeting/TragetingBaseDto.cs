using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Core;

namespace Noqoush.AdFalcon.Services.Interfaces.DTOs.Campaign.Targeting
{
    [DataContract]
    [KnownType(typeof(DeviceTargetingDto))]
    [KnownType(typeof(KeywordTargetingDto))]
    [KnownType(typeof(GeographicTargetingDto))]
    [KnownType(typeof(OperatorTargetingDto))]
    [KnownType(typeof(IPTargetingDto))]
    [KnownType(typeof(DemographicTargetingDto))]
    [KnownType(typeof(URLTargetingDto))]
    [KnownType(typeof(GeoFencingTargetingDto))]
    [KnownType(typeof(AdRequestTargetingDto))]
    [KnownType(typeof(ImpressionMetricTargetingDto))]
    [KnownType(typeof(LanguageTargetingDto))]
    [KnownType(typeof(VideoTargetingDto))]

    
    public class TargetingBaseDto
    {
        [DataMember]
        public int ID { get;  set; }
        [DataMember]
        public TargetingTypeDto Type { get; set; }
    }
}
