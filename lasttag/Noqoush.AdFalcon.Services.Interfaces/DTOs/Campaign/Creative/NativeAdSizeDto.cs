using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Noqoush.AdFalcon.Services.Interfaces.DTOs.Campaign.Creative
{
    [DataContract]
    [KnownType(typeof(NativeAdIconSizeDto))]
    [KnownType(typeof(NativeAdImageSizeDto))]
    public class NativeAdSizeDto
    {
        [DataMember]
        public int ID { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public virtual short Width { get; set; }
        [DataMember]
        public virtual short Height { get; set; }
        [DataMember]
        public virtual short Priority { get; set; }
        [DataMember]
        public virtual bool IsRequired { get; set; }
        [DataMember]
        public virtual List<NativeAdSizeFormatDto> Formats { get; set; }
    }
}

