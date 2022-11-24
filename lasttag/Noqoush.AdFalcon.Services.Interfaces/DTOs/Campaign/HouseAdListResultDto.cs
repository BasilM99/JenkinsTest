using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Noqoush.AdFalcon.Services.Interfaces.DTOs.Campaign
{
    [DataContract]
    public class HouseAdListResultDto
    {
        [DataMember]
        public IEnumerable<HouseAdBaseDto> Items { get; set; }
        [DataMember]
        public long TotalCount { get; set; }
    }
}
