using ArabyAds.AdFalcon.Domain.Common.Model.Campaign;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using ProtoBuf;
using System.Text;


namespace ArabyAds.AdFalcon.Services.Interfaces.DTOs.Campaign
{
    [ProtoContract]
    public class CreativeVendorDto : LookupDto
    {
       [ProtoMember(1)]
        public List<CreativeVendorKeywordDto> Keywords { get; set; } = new List<CreativeVendorKeywordDto>();
        [ProtoMember(2)]
        public List<CreativeVendorKeywordDto> InsertedKeywords { get; set; } = new List<CreativeVendorKeywordDto>();
        [ProtoMember(3)]
        public List<CreativeVendorKeywordDto> DeletedKeywords { get; set; } = new List<CreativeVendorKeywordDto>();
    }
}
