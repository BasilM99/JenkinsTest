using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Noqoush.Framework.DataAnnotations;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Core;
namespace Noqoush.AdFalcon.Services.Interfaces.DTOs.Campaign.Targeting
{
  
    [DataContract]
    public class AdRequestTypePlatformVersionDto 
    {
        [DataMember]
        public AdRequestTypeDto AdRequestType { get; set; }
        [DataMember]
        public AdRequestPlatformDto AdRequestPlatform { get; set; }
        [DataMember]
        public string Version { get; set; }

    }
}
