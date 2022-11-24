using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Noqoush.AdFalcon.Services.Interfaces.DTOs.Core
{
    public class AgeGroupDto : LookupDto
    {
        [DataMember]
        public int MinValue { get; set; }
        [DataMember]
        public int MaxValue { get; set; }
    }
}
