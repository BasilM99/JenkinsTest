using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Core;
using Noqoush.Framework.DataAnnotations;
using Noqoush.Framework.ExceptionHandling.Exceptions;

namespace Noqoush.AdFalcon.Services.Interfaces.DTOs.Account.Payment
{
    [DataContract]
    public class PaymentDtoResult
    {
        [DataMember]
        public IEnumerable<PaymentDto> Items { get; set; }

        [DataMember]
        public int Total { get; set; }
    }

    [DataContract]
    public class NewPaymentDto
    {
        [DataMember]
        [Required(ResourceName = "RequiredPaymentAccount", ResourceSet = "Global")]
        public int? AccountId { get; set; }

        [DataMember]
        public int? CurrencyId { get; set; }

        [DataMember]
        public string AccountName { get; set; }

        [DataMember]
        [Required(ResourceName = "RequiredAmount", ResourceSet = "Global")]
        [Range(0, 99999999, ResourceName = "MaxPayment")]
        [RegularExpression(@"^\$?\d+(\.(\d{1,2}))?$", ResourceName = "PaymentCurrencyMsg")]
        public decimal? Amount { get; set; }

        [DataMember]
        public decimal? VATAmount { get; set; }
        [DataMember]
        public double NetAmount
        {
            get; set;
        }


        [DataMember]
        [RegularExpression(@"^\$?\d+(\.(\d{1,2}))?$", ResourceName = "PaymentCurrencyMsg")]
        public decimal? OriginalAmount { get; set; }

        [DataMember]
        public string Comment { get; set; }

        [DataMember]

//        [Required(ResourceName = "RequiredMessage")]

        public string BeneficiaryName { get; set; }

        [DataMember]
        public string TransactionId { get; set; }

        [DataMember]
        public string AttachmentId { get; set; }

        [DataMember]
        public string AttachmentName { get; set; }

        [DataMember]
        //[Required(ResourceName = "RequiredMessage")]

        public string CheckNo { get; set; }

        [DataMember]
      //  [Required(ResourceName = "RequiredMessage")]

        public DateTime? DueDate { get; set; }

        [DataMember]
        public DateTime? ForMonth { get; set; }

        [DataMember]
        public int? SystemPaymentDetailId { get; set; }

        [DataMember]
        public int? AccountPaymentDetailId { get; set; }

        [DataMember]
        [Required(ResourceName = "RequiredPaymentDate", ResourceSet = "Global")]
        public DateTime? TransactionDate { get; set; }

        [DataMember]
        [Required(ResourceName = "RequiredMessage")]
        public int? PaymentType { get; set; }

        [DataMember]
        public bool NotifyUser { get; set; }


        public virtual BusinessException Validate()
        {
            var error = new BusinessException();
            if (!AccountId.HasValue)
            {
                error.Errors.Add(new ErrorData { ID = "RequiredPaymentAccount" });
            }
            if (!TransactionDate.HasValue)
            {
                error.Errors.Add(new ErrorData { ID = "RequiredPaymentDate" });
            }
            if (!PaymentType.HasValue)
            {
                error.Errors.Add(new ErrorData { ID = "RequiredPaymentType" });
            }
            if (!Amount.HasValue)
            {
                error.Errors.Add(new ErrorData { ID = "RequiredAmount" });
            }
            else
            {
                if (Amount <= 0 || Amount > 99999999)
                {
                    error.Errors.Add(new ErrorData { ID = "MaxPayment" });
                }
            }



            return error;
        }
    }

    [DataContract]
    public class PaymentDto
    {
        [DataMember]
        public decimal Amount { get; set; }

        [DataMember]
        public DateTime TransactionDate { get; set; }
        [DataMember]
        public string ReceiptNo { get; set; }
        [DataMember]
        public string AdFalconReceiptNo { get; set; }

        [DataMember]
        public PaymentTypeDto Type { get; set; }

        [DataMember]
        public string Comment { get; set; }
        [DataMember]
        public virtual decimal VATAmount { get; set; }

        [DataMember]
        public virtual decimal TotalAmount { get { return VATAmount + Amount; } set { } }
        //[DataMember]
        //public string TransactionId { get; set; }

        //[DataMember]
        //public string Payee { get; set; }

    }
}
