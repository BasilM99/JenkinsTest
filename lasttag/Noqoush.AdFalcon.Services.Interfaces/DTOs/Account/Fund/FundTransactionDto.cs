using System;
using System.Runtime.Serialization;

namespace Noqoush.AdFalcon.Services.Interfaces.DTOs.Account.Fund
{
    [DataContract]
    public class FundTransactionDto
    {

        
                    [DataMember]
        public virtual long TotalCount { get; set; }
        [DataMember]
        public virtual int AccountId { get; set; }

        [DataMember]
        public virtual decimal Amount { get; set; }


        [DataMember]
        public virtual string AmountText { get { return Amount.ToString("F2"); } set { } }

        [DataMember]
        public virtual decimal VATAmount { get; set; }

        [DataMember]
        public virtual string VATAmountText { get { return VATAmount.ToString("F2"); } set { } }
        [DataMember]
        public virtual string NoqoushReceiptNumber { get; set; }
        
        [DataMember]
        public virtual string Comment { get; set; }

        [DataMember]
        public virtual string TransactionId { get; set; }

        [DataMember]
        public virtual int CreatedById { get; set; }

        [DataMember]
        public virtual DateTime CreationDate { get; set; }

        [DataMember]
        public virtual int ID { get; set; }

        [DataMember]
        public virtual AccountFundTransStatusDto FundTransStatus { get; set; }

        [DataMember]
        public virtual AccountFundTransTypeDto FundTransType { get; set; }

        [DataMember]
        public virtual AccountFundTypeDto FundType { get; set; }

        [DataMember]
        public virtual string Payee { get; set; }

        [DataMember]
        public virtual DateTime TransactionDate { get; set; }

        [DataMember]
        public virtual int AccountFundPgwId { get; set; }


        [DataMember]
        public virtual string AccountName { get; set; }
        [DataMember]
        public virtual string Country { get; set; }
    }

}
