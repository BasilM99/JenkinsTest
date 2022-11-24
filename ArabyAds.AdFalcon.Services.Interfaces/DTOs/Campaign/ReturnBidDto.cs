using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProtoBuf;

namespace ArabyAds.AdFalcon.Services.Interfaces.DTOs.Campaign
{
    [ProtoContract]
    public class ReturnBidDto
    {
       [ProtoMember(1)]
        public IDictionary<int, decimal> CostModelsWrappersBidValues { get; set; } = new Dictionary<int, decimal>();

    }
}
