using System;
using ProtoBuf;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Reports;

namespace ArabyAds.AdFalcon.Services.Interfaces.DTOs.Campaign
{
    [ProtoContract]
    public class SitesListDto
    {
       [ProtoMember(1)]
        public int Id { get; set; }
       [ProtoMember(2)]
        public int BusinessId { get; set; }
       [ProtoMember(3)]
        public string Name { get; set; }

    }
}
