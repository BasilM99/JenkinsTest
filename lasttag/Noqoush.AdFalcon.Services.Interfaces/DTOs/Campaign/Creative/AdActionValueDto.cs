using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Core;

namespace Noqoush.AdFalcon.Services.Interfaces.DTOs.Campaign.Creative
{
    [DataContract]
    public class AdActionValueDto
    {
        [DataMember]

       [Noqoush.Framework.DataAnnotations.Required()]
        public string Value { get; set; }

        [DataMember]
        public string Value2 { get; set; }

        [DataMember]
        public IList<AdActionValueTrackerDto> Trackers { get; set; }
    }

}
