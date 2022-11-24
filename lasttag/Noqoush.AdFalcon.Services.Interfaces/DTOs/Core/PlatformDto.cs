using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Noqoush.AdFalcon.Services.Interfaces.DTOs.Core
{
    [DataContract]
    public class PlatformDto : LookupDto
    {
        [DataMember]
        public bool IsVisible { get; set; }

        [DataMember]
        public bool IsSelected { get; set; }

        [DataMember]
        public virtual List<PlatformVersionDto> Versions { get; set; }

        public PlatformDto ShallowCopy()
        {
            PlatformDto item = (PlatformDto)this.MemberwiseClone();
            item.Versions = Versions.Select(p => p.ShallowCopy()).ToList();

            return item;
        }
    }
}
