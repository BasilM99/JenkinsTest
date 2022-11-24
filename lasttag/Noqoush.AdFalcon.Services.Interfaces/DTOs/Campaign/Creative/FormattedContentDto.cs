using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Noqoush.AdFalcon.Services.Interfaces.DTOs.Campaign.Creative
{
    [DataContract]
    public class FormattedContentDto
    {
        [DataMember]
        public string Content { get; set; }

        [DataMember]
        public bool IsValid { get; set; }
    }
}
