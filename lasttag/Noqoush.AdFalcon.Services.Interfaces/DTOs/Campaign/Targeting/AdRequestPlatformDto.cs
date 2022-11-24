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
    public class AdRequestPlatformDto : LookupDto
    {
    
        [DataMember]
        
        public string Code { get; set; }
    
    }
}
