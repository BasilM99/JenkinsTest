using System;
using System.Collections.Generic;
using System.Linq;
using ProtoBuf;
using System.Text;

namespace ArabyAds.AdFalcon.Services.Interfaces.DTOs.Core
{
    [ProtoContract]
    public class PlatformVersionDto
    {
       [ProtoMember(1)]
        public virtual string Version { get; set; }

       [ProtoMember(2)]
        public virtual string Code { get; set; }

       [ProtoMember(3)]
        public virtual bool IsSelected { get; set; }

        public PlatformVersionDto ShallowCopy()
        {
            return (PlatformVersionDto)this.MemberwiseClone();
        }
    }
}
