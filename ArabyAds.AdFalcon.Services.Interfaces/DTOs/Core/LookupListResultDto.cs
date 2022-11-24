using System;
using System.Collections.Generic;
using System.Linq;
using ProtoBuf;
using System.Text;

namespace ArabyAds.AdFalcon.Services.Interfaces.DTOs.Core
{
    [ProtoContract]
    public class LookupListResultDto
    {
       [ProtoMember(1)]
        public IEnumerable<LookupDto> Items { get; set; } = new List<LookupDto>();


        [ProtoMember(2)]
        public long TotalCount { get; set; }

        [ProtoMember(3)]
        public IEnumerable<CostElementDto> CostElementItems { get; set; } = new List<CostElementDto>();
    }
}
