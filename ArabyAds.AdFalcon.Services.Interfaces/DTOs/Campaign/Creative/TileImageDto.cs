using System;
using System.Collections.Generic;
using ProtoBuf;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Core;

namespace ArabyAds.AdFalcon.Services.Interfaces.DTOs.Campaign.Creative
{
    [ProtoContract]
    public class TileImageDto : LookupDto
    {
       
       [ProtoMember(1)]
        public bool IsCustom { get; set; }
       [ProtoMember(2)]
        public IEnumerable<TileImageDocumentDto> Images  { get; set; } = new List<TileImageDocumentDto>();
    }
}
