using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProtoBuf;
using ArabyAds.Framework.DataAnnotations;

namespace ArabyAds.AdFalcon.Services.Interfaces.DTOs.Core
{
    [ProtoContract]
    public class LocationDto : LookupDto
    {
       [ProtoMember(1)]
        public int? ParentId { get; set; }

       [ProtoMember(2)]
        [Required]
        public LocationType Type { get; set; }

       [ProtoMember(3)]
        public IEnumerable<LocationDto> Locations { get; set; } = new List<LocationDto>();

        [ProtoMember(4)]
        [Required]
        [StringLength(2)]
        public virtual string TwoLettersCode { get; set; }

       [ProtoMember(5)]
        [Required]
        [StringLength(3)]
        public virtual string ThreeLettersCode { get; set; }

       [ProtoMember(6)]
        [StringLength(50)]
        public virtual string MobileCountryCode { get; set; }


        [ProtoMember(7)]
       public virtual bool IsDeleted { get; set; }
    }

    public enum LocationType
    {
        Continent = 1,
        Country = 2,
        State = 3,
        City = 4,
    }

    [ProtoContract]
    public class TreeDto
    {
       [ProtoMember(1)]
        public int CustomValue { get; set; }
       [ProtoMember(2)]
        public string CustomValue1 { get; set; }
       [ProtoMember(3)]
        public string CustomValue2 { get; set; }
       [ProtoMember(4)]
        public string Id { get; set; }
       [ProtoMember(5)]
        public string Key { get; set; }
       [ProtoMember(6)]
        public LocalizedStringDto Name { get; set; }
       [ProtoMember(7)]
        public List<TreeDto> Childs { get; set; } = new List<TreeDto>();

        [ProtoMember(8)]
        public string state { get; set; }

       [ProtoMember(9)]
        public string style { get; set; }
    }
}
