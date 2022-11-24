using System;
using System.Collections.Generic;
using ProtoBuf;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Core;
using ArabyAds.Framework.DataAnnotations;
using ArabyAds.Framework.ExceptionHandling.Exceptions;

namespace ArabyAds.AdFalcon.Services.Interfaces.DTOs.Account.Payment
{
    [ProtoContract]
    public class PaymentDtoResult
    {
        [ProtoMember(1)]
        public IEnumerable<PaymentDto> Items { get; set; } = new List<PaymentDto>();

        [ProtoMember(2)]
        public int Total { get; set; }
    }

    [ProtoContract]
    public class NewPaymentDto
    {
        [ProtoMember(1)]
        [Required(ResourceName = "RequiredPaymentAccount", ResourceSet = "Global")]
        public int? AccountId { get; set; }

        [ProtoMember(2)]
        public int? CurrencyId { get; set; }

        [ProtoMember(3)]
        public string AccountName { get; set; }

        [ProtoMember(4)]
        [Required(ResourceName = "RequiredAmount", ResourceSet = "Global")]
        [Range(0, 99999999, ResourceName = "MaxPayment")]
        [RegularExpression(@"^\$?\d+(\.(\d{1,2}))?$", ResourceName = "PaymentCurrencyMsg")]
        public decimal? Amount { get; set; }

        [ProtoMember(5)]
        public decimal? VATAmount { get; set; }
        [ProtoMember(6)]
        public double NetAmount
        {
            get; set;
        }


        [ProtoMember(7)]
        [RegularExpression(@"^\$?\d+(\.(\d{1,2}))?$", ResourceName = "PaymentCurrencyMsg")]
        public decimal? OriginalAmount { get; set; }

        [ProtoMember(8)]
        public string Comment { get; set; }

        [ProtoMember(9)]

        //        [Required(ResourceName = "RequiredMessage")]

        public string BeneficiaryName { get; set; }

        [ProtoMember(10)]
        public string TransactionId { get; set; }

        [ProtoMember(11)]
        public string AttachmentId { get; set; }

        [ProtoMember(12)]
        public string AttachmentName { get; set; }

        [ProtoMember(13)]
        //[Required(ResourceName = "RequiredMessage")]

        public string CheckNo { get; set; }

        [ProtoMember(14)]
        //  [Required(ResourceName = "RequiredMessage")]

        public DateTime? DueDate { get; set; }

        [ProtoMember(15)]
        public DateTime? ForMonth { get; set; }

        [ProtoMember(16)]
        public int? SystemPaymentDetailId { get; set; }

        [ProtoMember(17)]
        public int? AccountPaymentDetailId { get; set; }

        [ProtoMember(18)]
        [Required(ResourceName = "RequiredPaymentDate", ResourceSet = "Global")]
        public DateTime? TransactionDate { get; set; }

        [ProtoMember(19)]
        [Required(ResourceName = "RequiredMessage")]
        public int? PaymentType { get; set; }

        [ProtoMember(20)]
        public bool NotifyUser { get; set; }



    }

    [ProtoContract]
    public class PaymentDto
    {
        [ProtoMember(1)]
        public decimal Amount { get; set; }

        [ProtoMember(2)]
        public DateTime TransactionDate { get; set; }
        [ProtoMember(3)]
        public string ReceiptNo { get; set; }
        [ProtoMember(4)]
        public string AdFalconReceiptNo { get; set; }

        [ProtoMember(5)]
        public PaymentTypeDto Type { get; set; }

        [ProtoMember(6)]
        public string Comment { get; set; }
        [ProtoMember(7)]
        public virtual decimal VATAmount { get; set; }

        public string VATAmountDisplay
        {
            get
            {
                if (VATAmount == 0) return null;
                return VATAmount.ToString();
            }
        }

        [ProtoMember(8)]
        public virtual decimal TotalAmount { get { return VATAmount + Amount; } set { } }
        //[DataMember]
        //public string TransactionId { get; set; }

        //[DataMember]
        //public string Payee { get; set; }

        [ProtoMember(9)]
        public virtual string VATAmountString { get; set; }

    }
}
