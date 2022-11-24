using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;

namespace ArabyAds.AdFalcon.Services.Interfaces.Messages
{
    [ProtoContract]
    public class AddOverBudgetReturnFundRequest
    {
        [ProtoMember(1)]
        public int Id { set; get; }
        [ProtoMember(2)]
        public decimal InvoiceAmount { set; get; }
    }
}
