using System;
using ProtoBuf;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Reports;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Core;

namespace ArabyAds.AdFalcon.Services.Interfaces.DTOs.Campaign
{
    [ProtoContract]
    public class MetricVendorDto : LookupDto
    {
       [ProtoMember(1)]
        public string Code { get; set; }
       [ProtoMember(2)]
        public string Description { get; set; }
    }
}
