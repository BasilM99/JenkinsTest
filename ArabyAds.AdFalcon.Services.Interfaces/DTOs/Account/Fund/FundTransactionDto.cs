using System;
using ProtoBuf;

namespace ArabyAds.AdFalcon.Services.Interfaces.DTOs.Account.Fund
{
    [ProtoContract]
    public class FundTransactionDto
    {

        
        [ProtoMember(1)]
        public virtual long TotalCount { get; set; }
       [ProtoMember(2)]
        public virtual int AccountId { get; set; }

       [ProtoMember(3)]
        public virtual decimal Amount { get; set; }


       [ProtoMember(4)]
        public virtual string AmountText { get { return Amount.ToString("F2"); } set { } }

       [ProtoMember(5)]
        public virtual decimal VATAmount { get; set; }

       [ProtoMember(6)]
        public virtual string VATAmountText { get { return VATAmount.ToString("F2"); } set { } }
       [ProtoMember(7)]
        public virtual string NoqoushReceiptNumber { get; set; }
        
       [ProtoMember(8)]
        public virtual string Comment { get; set; }

       [ProtoMember(9)]
        public virtual string TransactionId { get; set; }

       [ProtoMember(10)]
        public virtual int CreatedById { get; set; }

       [ProtoMember(11)]
        public virtual DateTime CreationDate { get; set; }

       [ProtoMember(12)]
        public virtual int ID { get; set; }

       [ProtoMember(13)]
        public virtual AccountFundTransStatusDto FundTransStatus { get; set; }

       [ProtoMember(14)]
        public virtual AccountFundTransTypeDto FundTransType { get; set; }

       [ProtoMember(15)]
        public virtual AccountFundTypeDto AccountFundType { get; set; }

       [ProtoMember(16)]
        public virtual string Payee { get; set; }

       [ProtoMember(17)]
        public virtual DateTime TransactionDate { get; set; }

       [ProtoMember(18)]
        public virtual int AccountFundPgwId { get; set; }


       [ProtoMember(19)]
        public virtual string AccountName { get; set; }
       [ProtoMember(20)]
        public virtual string Country { get; set; }
    }

}
