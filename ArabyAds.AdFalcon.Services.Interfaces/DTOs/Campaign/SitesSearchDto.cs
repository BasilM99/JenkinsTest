using ProtoBuf;
using System.Collections.Generic;
using System;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Reports;

namespace ArabyAds.AdFalcon.Services.Interfaces.DTOs.Campaign
{
    [ProtoContract]
    public class SitesSearchDto
    {
       [ProtoMember(1)]
        public string BusinessName { get; set; }

       [ProtoMember(2)]
        public IEnumerable<SitesListDto> Items { get; set; }
       [ProtoMember(3)]
        public long TotalCount { get; set; }
    }

}
