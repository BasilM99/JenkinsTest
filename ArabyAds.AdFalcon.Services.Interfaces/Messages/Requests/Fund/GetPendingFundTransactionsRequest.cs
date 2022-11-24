using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;

namespace ArabyAds.AdFalcon.Services.Interfaces.Messages
{
    [ProtoContract]
    public class GetPendingFundTransactionsRequest
    {
        [ProtoMember(1)]
        public int fundTransactionTypeId { set; get; }
        [ProtoMember(2)]
        public DateTime DateTo { set; get; }

        public override string ToString()
        {
            return $"{fundTransactionTypeId}_{DateTo}";
        }
    }
}
