using ArabyAds.AdFalcon.Domain.Common.Model.Account;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;

namespace ArabyAds.AdFalcon.Services.Interfaces.Messages
{
    [ProtoContract]
    public class GetFullPaymentDetailsRequest : GetPaymentDetailsRequest
    {
        [ProtoMember(1)]
        public PayemntAccountSubType PaymentAccountSubType { get; set; }

        public override string ToString()
        {
            return $"{base.ToString()}_{PaymentAccountSubType}";
        }
    }
}
