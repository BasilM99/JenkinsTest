using Noqoush.Framework.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Noqoush.AdFalcon.Services.Interfaces.DTOs.Campaign.Targeting
{
    [DataContract]
    public class GeoFencingTargetingDto : TargetingBaseDto
    {
        [DataMember]
        [Required]
        [Range(-90, 90)]
        public virtual decimal Latitude { get; set; }

        [DataMember]
        [Required]
        [Range(-180,180)]
        public virtual decimal Longitude { get; set; }

        [DataMember]
        [Required]
        [Range(0, double.MaxValue)]
        public virtual decimal Radius { get; set; }

    }

    public class GeoFencingUITargeting
    {
        public virtual string Latitude { get; set; }

        public virtual string Longitude { get; set; }

        public virtual string Radius { get; set; }
    }
}
