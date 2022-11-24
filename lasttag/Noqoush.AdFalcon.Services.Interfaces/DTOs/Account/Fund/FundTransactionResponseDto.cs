using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Noqoush.AdFalcon.Services.Interfaces.DTOs.Account.Fund
{
    [DataContract]
    [KnownType(typeof(PgwFundTransactionResponseDto))]
    [KnownType(typeof(PayPalFundTransactionResponseDto))]
    public class FundTransactionResponseDto
    {
        [DataMember]
        public virtual int ID
        {
            get;
            set;
        }

        
        [DataMember]
        public virtual string TransactionId
        {
            get;
            set;
        }


        [DataMember]
        public virtual AccountFundTransStatusDto FundTransStatus
        {
            get;
            set;
        }

        [DataMember]
        public virtual decimal Amount
        {
            get;
            set;
        }
        [DataMember]
        public virtual decimal VATAmount
        {
            get;
            set;
        }

        [DataMember]
        public virtual DateTime TransactionDate
        {
            get;
            set;
        }
    }

    [DataContract]
    public class PgwFundTransactionResponseDto : FundTransactionResponseDto
    {
        [DataMember]
        public virtual string ReceiptNumber
        {
            get;
            set;
        }


        [DataMember]
        public virtual string ErrorCode
        {
            get;
            set;
        }
        [DataMember]
        public virtual string ExtraInfo
        {
            get;
            set;
        }
        [DataMember]
        public virtual string PgwStatus
        {
            get;
            set;
        }
        [DataMember]
        public virtual DateTime? ResponseDate
        {
            get;
            set;
        }

    }

    [DataContract]
    public class PayPalFundTransactionResponseDto : FundTransactionResponseDto
    {

        [DataMember]
        public virtual string EmailAddress
        {
            get;
            set;
        }

    }
}
