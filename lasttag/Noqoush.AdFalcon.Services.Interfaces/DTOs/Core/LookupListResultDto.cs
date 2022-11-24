using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Noqoush.AdFalcon.Services.Interfaces.DTOs.Core
{
    [DataContract]
    public class LookupListResultDto
    {
        [DataMember]
        public IEnumerable<LookupDto> Items { get; set; }
        [DataMember]
        public long TotalCount { get; set; }
    }
}
