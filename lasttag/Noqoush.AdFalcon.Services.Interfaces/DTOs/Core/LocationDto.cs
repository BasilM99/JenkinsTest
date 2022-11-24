using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Noqoush.Framework.DataAnnotations;

namespace Noqoush.AdFalcon.Services.Interfaces.DTOs.Core
{
    [DataContract]
    public class LocationDto : LookupDto
    {
        [DataMember]
        public int? ParentId { get; set; }

        [DataMember]
        [Required]
        public LocationType Type { get; set; }

        [DataMember]
        public IEnumerable<LocationDto> Locations { get; set; }

        [DataMember]
        [Required]
        [StringLength(2)]
        public virtual string TwoLettersCode { get; set; }

        [DataMember]
        [Required]
        [StringLength(3)]
        public virtual string ThreeLettersCode { get; set; }

        [DataMember]
        [StringLength(50)]
        public virtual string MobileCountryCode { get; set; }
    }

    [DataContract]
    public enum LocationType
    {
        [EnumMember]
        Continent = 1,
        [EnumMember]
        Country = 2,
        [EnumMember]
        State = 3,
        [EnumMember]
        City = 4,
    }

    [DataContract]
    public class TreeDto
    {
        [DataMember]
        public int CustomValue { get; set; }
        [DataMember]
        public string CustomValue1 { get; set; }
        [DataMember]
        public string CustomValue2 { get; set; }
        [DataMember]
        public string Id { get; set; }
        [DataMember]
        public string Key { get; set; }
        [DataMember]
        public LocalizedStringDto Name { get; set; }
        [DataMember]
        public List<TreeDto> Childs { get; set; }

        [DataMember]
        public string state { get; set; }

        [DataMember]
        public string style { get; set; }
    }
}
