using ArabyAds.AdFalcon.Domain.Common.Model.Account;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;

namespace ArabyAds.AdFalcon.Services.Interfaces.Messages
{
    [ProtoContract]
    [ProtoInclude(100, typeof(GetFullPaymentDetailsRequest))]
    public class GetPaymentDetailsRequest
    {
        [ProtoMember(1)]
        public int AccountId { get; set; }
        [ProtoMember(2)]
        public PayemntAccountType PaymentAccountType { get; set; }

        public override string ToString()
        {
            return $"{AccountId}_{PaymentAccountType}";
        }
    }
}
