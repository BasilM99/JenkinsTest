using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;

namespace ArabyAds.AdFalcon.Services.Interfaces.Messages
{
    [ProtoContract]
    public class SendBillingInfoacknowledgmentRequest
    {
        [ProtoMember(1)]
        public int Id { set; get; }

        [ProtoMember(2)]
        public string FieldToChange { set; get; }

        [ProtoMember(3)]
        public decimal? RequestedAmount { set; get; }

        [ProtoMember(4)]
        public decimal? CommittedAmount { set; get; }

        [ProtoMember(5)]
        public DateTime ModifiedOn { set; get; }
    }
}
