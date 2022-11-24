using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Noqoush.AdFalcon.Services.Interfaces.DTOs.Campaign.Targeting
{
    [DataContract]
    public class URLTargetingDto : TargetingBaseDto
    {
        [DataMember]
        public string URL { get; set; }
    }
}
