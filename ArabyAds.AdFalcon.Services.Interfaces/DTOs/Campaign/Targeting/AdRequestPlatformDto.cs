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
    public class AdRequestPlatformDto : LookupDto
    {
    
       [ProtoMember(1)]
        
        public string Code { get; set; }
    
    }
}
