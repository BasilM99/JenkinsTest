using ArabyAds.AdFalcon.Services.Interfaces.DTOs.AppSite;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;

namespace ArabyAds.AdFalcon.Services.Interfaces.Messages
{
    [ProtoContract]
    public class AppSiteLanguageFilterMessage
    {
        [ProtoMember(1)]
        public LanguageFilterDto LanguageFilterDto { get; set; }
        [ProtoMember(2)]
        public int AppSiteId { get; set; }
      
    }
}
