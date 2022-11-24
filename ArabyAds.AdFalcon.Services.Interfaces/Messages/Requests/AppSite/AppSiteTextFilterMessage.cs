using ArabyAds.AdFalcon.Services.Interfaces.DTOs.AppSite;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;

namespace ArabyAds.AdFalcon.Services.Interfaces.Messages
{
    [ProtoContract]
    public class AppSiteTextFilterMessage
    {
        [ProtoMember(1)]
        public TextFilterDto TextFilterDto { get; set; }
        [ProtoMember(2)]
        public int AppSiteId { get; set; }
      
    }
}
