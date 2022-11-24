using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Account.SSP;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Campaign;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;

namespace ArabyAds.AdFalcon.Services.Interfaces.Messages
{
    [ProtoContract]
    public class SaveSiteZoneMappingRequest
    {
        [ProtoMember(1)]
        public SiteZoneMappingDto SiteZoneMappingDto { get; set; }
        [ProtoMember(2)]
        public IList<AssignedAppsitesDto> AppSites { get; set; }
    }
}
