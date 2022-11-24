using Noqoush.AdFalcon.Domain.Common.Model.Campaign;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;


namespace Noqoush.AdFalcon.Services.Interfaces.DTOs.Campaign
{
    [DataContract]
    public class CreativeVendorDto : LookupDto
    {
        [DataMember]
        public List<CreativeVendorKeywordDto> Keywords { get; set; }
        [DataMember]
        public List<CreativeVendorKeywordDto> InsertedKeywords { get; set; }
        [DataMember]
        public List<CreativeVendorKeywordDto> DeletedKeywords { get; set; }
    }
}
