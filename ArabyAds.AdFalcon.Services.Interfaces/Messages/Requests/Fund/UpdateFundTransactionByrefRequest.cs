using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;

namespace ArabyAds.AdFalcon.Services.Interfaces.Messages
{
    [ProtoContract]
    public class UpdateFundTransactionByrefRequest
    {
        [ProtoMember(1)]
        public int Id { set; get; }
        [ProtoMember(2)]
        public string ReferenceId { set; get; }
    }
}
