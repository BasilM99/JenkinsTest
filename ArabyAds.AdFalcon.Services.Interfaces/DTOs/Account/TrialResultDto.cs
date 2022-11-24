using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArabyAds.AdFalcon.Services.Interfaces.DTOs.Account
{
    [ProtoContract]
    public class TrialResultDto
    {
        [ProtoMember(1)]
        public IEnumerable<TrialDto> Items { get; set; } = new List<TrialDto>();
        [ProtoMember(2)]

        public long TotalCount { get; set; }

    }
}
