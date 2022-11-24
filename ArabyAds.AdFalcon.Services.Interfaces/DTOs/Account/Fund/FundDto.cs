using System;
using System.Collections.Generic;
using ProtoBuf;
using ArabyAds.AdFalcon.Domain.Common.Model.Account;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Account.Payment;
using ArabyAds.Framework.DataAnnotations;
using ArabyAds.Framework.ExceptionHandling.Exceptions;

namespace ArabyAds.AdFalcon.Services.Interfaces.DTOs.Account.Fund
{
    [ProtoContract]
    public class FundDtoResult
    {
       [ProtoMember(1)]
        public IEnumerable<NewFundDto> Items { get; set; }

       [ProtoMember(2)]
        public int Total { get; set; }
    }

    [ProtoContract]
    public class NewFundDto
    {
       [ProtoMember(1)]
        public int? ObjectRelatedId { get; set; }
       [ProtoMember(2)]
        [Required(ResourceName = "RequiredPaymentAccount", ResourceSet = "Global")]
        public int? AccountId { get; set; }

       [ProtoMember(3)]
        public int? CurrencyId { get; set; }
       [ProtoMember(4)]
        [Required]
        public int TypeId { get; set; }

       [ProtoMember(5)]
        public string AccountName { get; set; }

       [ProtoMember(6)]
        [Required(ResourceName = "RequiredAmount", ResourceSet = "Global")]
        [Range(1, 99999999, ResourceName = "MaxPayment")]
        [RegularExpression(@"^\$?\d+(\.(\d{1,2}))?$", ResourceName = "PaymentCurrencyMsg")]
        public decimal? Amount { get; set; }
       [ProtoMember(7)]

        public decimal? VatAmount
        {
            get; set;
        }
       [ProtoMember(8)]
        public double NetAmount
        {
            get; set;
        }

       [ProtoMember(9)]
        [RegularExpression(@"^\$?\d+(\.(\d{1,2}))?$", ResourceName = "PaymentCurrencyMsg")]
        public decimal? OriginalAmount { get; set; }


       [ProtoMember(10)]
        public AccountPaymentDetailFundDto PaymentDetail { get; set; }

       [ProtoMember(11)]
        public string Comment { get; set; }

       [ProtoMember(12)]
       // [Required(ResourceName = "RequiredMessage")]

        public string IssuerName { get; set; }

       [ProtoMember(13)]
      //  [Required(ResourceName = "RequiredMessage")]

        public string IssuerBankName { get; set; }

       [ProtoMember(14)]
      //  [Required(ResourceName = "RequiredMessage")]

        public string IssuerBankBranch { get; set; }

       [ProtoMember(15)]
        public string TransactionId { get; set; }

       [ProtoMember(16)]
        public string AttachmentId { get; set; }

       [ProtoMember(17)]
        public string AttachmentName { get; set; }


       [ProtoMember(18)]
      //  [Required(ResourceName = "RequiredMessage")]

        public string CheckNo { get; set; }

       [ProtoMember(19)]
        //[Required(ResourceName = "RequiredMessage")]

        public DateTime? DueDate { get; set; }

       [ProtoMember(20)]
        public int? SystemPaymentDetailId { get; set; }

       [ProtoMember(21)]
        public int? AccountPaymentDetailId { get; set; }

       [ProtoMember(22)]
        [Required(ResourceName = "RequiredPaymentDate", ResourceSet = "Global")]

        public DateTime? TransactionDate { get; set; }

       [ProtoMember(23)]
        [Required(ResourceName = "RequiredMessage")]

        public int? FundType { get; set; }

       [ProtoMember(24)]
        public bool NotifyUser { get; set; }

 
    }

    [ProtoContract]
    public class FundDto
    {
       [ProtoMember(1)]
        public decimal Amount { get; set; }
       [ProtoMember(2)]
        public decimal VATAmount { get; set; }
       [ProtoMember(3)]
        public DateTime TransactionDate { get; set; }
       [ProtoMember(4)]
        public string ReceiptNo { get; set; }
       [ProtoMember(5)]
        public string AdFalconReceiptNo { get; set; }

       [ProtoMember(6)]
        public PaymentTypeDto Type { get; set; }
    }
}
