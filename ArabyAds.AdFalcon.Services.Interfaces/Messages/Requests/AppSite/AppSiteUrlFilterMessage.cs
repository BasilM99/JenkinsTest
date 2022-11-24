using ArabyAds.AdFalcon.Services.Interfaces.DTOs.AppSite;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;

namespace ArabyAds.AdFalcon.Services.Interfaces.Messages
{
    [ProtoContract]
    public class AppSiteUrlFilterMessage
    {
        [ProtoMember(1)]
        public UrlFilterDto UrlFilterDto { get; set; }
        [ProtoMember(2)]
        public int AppSiteId { get; set; }
      
    }
}
