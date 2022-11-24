using ArabyAds.Framework.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using ProtoBuf;
using System.Text;

namespace ArabyAds.AdFalcon.Services.Interfaces.DTOs.Campaign.Targeting
{
    [ProtoContract]
    public class GeoFencingTargetingDto : TargetingBaseDto
    {
       [ProtoMember(1)]
        [Required]
        [Range(-90, 90)]
        public virtual decimal Latitude { get; set; }

       [ProtoMember(2)]
        [Required]
        [Range(-180,180)]
        public virtual decimal Longitude { get; set; }

       [ProtoMember(3)]
        [Required]

        [Range(1, double.MaxValue)]
        public virtual decimal Radius { get; set; }

    }
    [ProtoContract]
    public class GeoFencingUITargeting
    {
        [ProtoMember(1)]
        public virtual decimal Latitude { get; set; }
        [ProtoMember(2)]
        public virtual decimal Longitude { get; set; }

        [ProtoMember(3)]
        public virtual decimal Radius { get; set; }


        [ProtoMember(4)]
        public int ID { get; set; }
    }
}