using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProtoBuf;

namespace ArabyAds.AdFalcon.Services.Interfaces.DTOs.Core
{
    [ProtoContract]
    public class PlatformDto : LookupDto
    {
       [ProtoMember(1)]
        public bool IsVisible { get; set; }

       [ProtoMember(2)]
        public bool IsSelected { get; set; }

        [ProtoMember(3)]
        public virtual List<PlatformVersionDto> Versions { get; set; } = new List<PlatformVersionDto>();

        public PlatformDto ShallowCopy()
        {
            PlatformDto item = (PlatformDto)this.MemberwiseClone();
            if (item != null && item.Versions!=null)
            {
                item.Versions = Versions.Select(p => p.ShallowCopy()).ToList();
            }

            return item;
        }
    }
}
