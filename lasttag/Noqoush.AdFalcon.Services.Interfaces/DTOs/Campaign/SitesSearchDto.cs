using System.Runtime.Serialization;
using System.Collections.Generic;
using System;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Reports;

namespace Noqoush.AdFalcon.Services.Interfaces.DTOs.Campaign
{
    [DataContract]
    public class SitesSearchDto
    {
        [DataMember]
        public string BusinessName { get; set; }

        [DataMember]
        public IEnumerable<SitesListDto> Items { get; set; }
        [DataMember]
        public long TotalCount { get; set; }
    }

}
