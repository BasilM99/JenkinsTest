using System;
using ProtoBuf;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Core;

namespace ArabyAds.AdFalcon.Services.Interfaces.DTOs.Campaign.Creative
{
    [ProtoContract]
    public class TileImageDocumentDto : LookupDto
    {

       [ProtoMember(1)]
        public virtual DocumentBaseDto Document { get; set; }
       [ProtoMember(2)]
        public virtual TileImageSizeDto TileImageSize { get; set; }
       [ProtoMember(3)]
        public virtual bool IsDeleted { get; set; }
    }
}
