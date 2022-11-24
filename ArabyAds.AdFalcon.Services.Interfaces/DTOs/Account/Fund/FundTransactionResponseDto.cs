using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProtoBuf;

namespace ArabyAds.AdFalcon.Services.Interfaces.DTOs.Account.Fund
{
    [ProtoContract]
    [ProtoInclude(100,typeof(PgwFundTransactionResponseDto))]
    [ProtoInclude(101,typeof(PayPalFundTransactionResponseDto))]
    public class FundTransactionResponseDto
    {
       [ProtoMember(1)]
        public virtual int ID
        {
            get;
            set;
        }

        
       [ProtoMember(2)]
        public virtual string TransactionId
        {
            get;
            set;
        }


       [ProtoMember(3)]
        public virtual AccountFundTransStatusDto FundTransStatus
        {
            get;
            set;
        }

       [ProtoMember(4)]
        public virtual decimal Amount
        {
            get;
            set;
        }
       [ProtoMember(5)]
        public virtual decimal VATAmount
        {
            get;
            set;
        }

       [ProtoMember(6)]
        public virtual DateTime TransactionDate
        {
            get;
            set;
        }
    }

    [ProtoContract]
    public class PgwFundTransactionResponseDto : FundTransactionResponseDto
    {
       [ProtoMember(1)]
        public virtual string ReceiptNumber
        {
            get;
            set;
        }


       [ProtoMember(2)]
        public virtual string ErrorCode
        {
            get;
            set;
        }
       [ProtoMember(3)]
        public virtual string ExtraInfo
        {
            get;
            set;
        }
       [ProtoMember(4)]
        public virtual string PgwStatus
        {
            get;
            set;
        }
       [ProtoMember(5)]
        public virtual DateTime? ResponseDate
        {
            get;
            set;
        }

    }

    [ProtoContract]
    public class PayPalFundTransactionResponseDto : FundTransactionResponseDto
    {

       [ProtoMember(1)]
        public virtual string EmailAddress
        {
            get;
            set;
        }

    }
}
