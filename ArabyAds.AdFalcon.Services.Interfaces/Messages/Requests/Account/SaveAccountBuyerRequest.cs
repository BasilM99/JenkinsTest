using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;

namespace ArabyAds.AdFalcon.Services.Interfaces.Messages
{
    [ProtoContract]
    public class SaveAccountBuyerRequest
    {
        [ProtoMember(1)]
        public string BuyerCode { get; set; }
        [ProtoMember(2)]
        public int? BuyerId { get; set; }
      
    }
}
