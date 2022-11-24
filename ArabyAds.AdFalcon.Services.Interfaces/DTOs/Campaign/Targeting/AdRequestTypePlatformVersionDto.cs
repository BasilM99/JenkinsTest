using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProtoBuf;
using ArabyAds.Framework.DataAnnotations;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Core;
namespace ArabyAds.AdFalcon.Services.Interfaces.DTOs.Campaign.Targeting
{
  
    [ProtoContract]
    public class AdRequestTypePlatformVersionDto 
    {
       [ProtoMember(1)]
        public AdRequestTypeDto AdRequestType { get; set; }
       [ProtoMember(2)]
        public AdRequestPlatformDto AdRequestPlatform { get; set; }
       [ProtoMember(3)]
        public string Version { get; set; }

    }
}
