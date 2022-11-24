using System;
using System.Collections.Generic;
using ProtoBuf;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Core;

namespace ArabyAds.AdFalcon.Services.Interfaces.DTOs.Campaign.Creative
{
    [ProtoContract]
    public class TileImageSizeDto : LookupDto
    {
       [ProtoMember(1)]
        public int Width { get; set; }
       [ProtoMember(2)]
        public int Height { get; set; }
       [ProtoMember(3)]
        public virtual bool IsActionTile { get; set; }
       [ProtoMember(4)]
        public virtual TileImageSizeDto TitleSize { get; set; }
       [ProtoMember(5)]
        public IList<FormatDto> Formats { get; set; } = new List<FormatDto>();
        [ProtoMember(6)]
        public int DeviceType { get; set; }
    }
}
