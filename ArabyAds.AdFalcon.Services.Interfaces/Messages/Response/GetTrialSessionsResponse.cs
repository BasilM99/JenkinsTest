using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Account;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;

namespace ArabyAds.AdFalcon.Services.Interfaces.Messages
{
    [ProtoContract]
    public class GetTrialSessionsResponse
    {
        [ProtoMember(1)]
        public IList<TrialDto> Trials { get; set; }  = new List<TrialDto>();

        [ProtoMember(2)]
        public int Total { get; set; }
    }
}
