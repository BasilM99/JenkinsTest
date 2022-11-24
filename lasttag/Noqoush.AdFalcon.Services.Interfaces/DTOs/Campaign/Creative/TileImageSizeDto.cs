using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Core;

namespace Noqoush.AdFalcon.Services.Interfaces.DTOs.Campaign.Creative
{
    [DataContract]
    public class TileImageSizeDto : LookupDto
    {
        [DataMember]
        public int Width { get; set; }
        [DataMember]
        public int Height { get; set; }
        [DataMember]
        public virtual bool IsActionTile { get; set; }
        [DataMember]
        public virtual TileImageSizeDto TitleSize { get; set; }
        [DataMember]
        public IList<FormatDto> Formats { get; set; }
        [DataMember]
        public int DeviceType { get; set; }
    }
}
