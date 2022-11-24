using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Noqoush.AdFalcon.Services.Interfaces.DTOs.Core
{
    [DataContract]
    public class PlatformVersionDto
    {
        [DataMember]
        public virtual string Version { get; set; }

        [DataMember]
        public virtual string Code { get; set; }

        [DataMember]
        public virtual bool IsSelected { get; set; }

        public PlatformVersionDto ShallowCopy()
        {
            return (PlatformVersionDto)this.MemberwiseClone();
        }
    }
}
