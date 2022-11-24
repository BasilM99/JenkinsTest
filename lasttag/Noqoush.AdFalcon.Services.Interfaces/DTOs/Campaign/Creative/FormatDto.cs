using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Core;

namespace Noqoush.AdFalcon.Services.Interfaces.DTOs.Campaign.Creative
{
    [DataContract]
    public class FormatDto : LookupDto
    {
        [DataMember]
        public int ID { get; set; }
        [DataMember]
        public string Format { get; set; }

        [DataMember]
        public virtual int MaxSize { get; set; }
    }
}
