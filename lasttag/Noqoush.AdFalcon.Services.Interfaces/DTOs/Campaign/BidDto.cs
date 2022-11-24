using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Noqoush.AdFalcon.Services.Interfaces.DTOs.Campaign
{
    [DataContract]
    public class BidDto
    {
        [DataMember]
        public int[] Operators { get; set; }
        [DataMember]
        public int[] Geographies { get; set; }
        [DataMember]
        public int[] Manufacturers { get; set; }
        [DataMember]
        public int[] Platforms { get; set; }
        [DataMember]
        public int[] Keywords { get; set; }
        [DataMember]
        public int[] Models { get; set; }
        [DataMember]
        public int? Demographic { get; set; }
        [DataMember]
        public int DeviceTargetingTypeId { get; set; }
        [DataMember]
        public int ActionType { get; set; }
        [DataMember]
        public int? AdTypeId { get; set; }
        [DataMember]
        public int[] DeviceCapabilities { get; set; }

    }
}
