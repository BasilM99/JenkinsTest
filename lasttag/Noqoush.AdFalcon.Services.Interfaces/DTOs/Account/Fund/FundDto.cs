using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Noqoush.AdFalcon.Domain.Common.Model.Account;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Account.Payment;
using Noqoush.Framework.DataAnnotations;
using Noqoush.Framework.ExceptionHandling.Exceptions;

namespace Noqoush.AdFalcon.Services.Interfaces.DTOs.Account.Fund
{
    [DataContract]
    public class FundDtoResult
    {
        [DataMember]
        public IEnumerable<NewFundDto> Items { get; set; }

        [DataMember]
        public int Total { get; set; }
    }

    [DataContract]
    public class NewFundDto
    {
        [DataMember]
        public int? ObjectRelatedId { get; set; }
        [DataMember]
        [Required(ResourceName = "RequiredPaymentAccount", ResourceSet = "Global")]
        public int? AccountId { get; set; }

        [DataMember]
        public int? CurrencyId { get; set; }
        [DataMember]
        [Required]
        public int TypeId { get; set; }

        [DataMember]
        public string AccountName { get; set; }

        [DataMember]
        [Required(ResourceName = "RequiredAmount", ResourceSet = "Global")]
        [Range(1, 99999999, ResourceName = "MaxPayment")]
        [RegularExpression(@"^\$?\d+(\.(\d{1,2}))?$", ResourceName = "PaymentCurrencyMsg")]
        public decimal? Amount { get; set; }
        [DataMember]

        public decimal? VatAmount
        {
            get; set;
        }
        [DataMember]
        public double NetAmount
        {
            get; set;
        }

        [DataMember]
        [RegularExpression(@"^\$?\d+(\.(\d{1,2}))?$", ResourceName = "PaymentCurrencyMsg")]
        public decimal? OriginalAmount { get; set; }


        [DataMember]
        public AccountPaymentDetailFundDto PaymentDetail { get; set; }

        [DataMember]
        public string Comment { get; set; }

        [DataMember]
       // [Required(ResourceName = "RequiredMessage")]

        public string IssuerName { get; set; }

        [DataMember]
      //  [Required(ResourceName = "RequiredMessage")]

        public string IssuerBankName { get; set; }

        [DataMember]
      //  [Required(ResourceName = "RequiredMessage")]

        public string IssuerBankBranch { get; set; }

        [DataMember]
        public string TransactionId { get; set; }

        [DataMember]
        public string AttachmentId { get; set; }

        [DataMember]
        public string AttachmentName { get; set; }


        [DataMember]
      //  [Required(ResourceName = "RequiredMessage")]

        public string CheckNo { get; set; }

        [DataMember]
        //[Required(ResourceName = "RequiredMessage")]

        public DateTime? DueDate { get; set; }

        [DataMember]
        public int? SystemPaymentDetailId { get; set; }

        [DataMember]
        public int? AccountPaymentDetailId { get; set; }

        [DataMember]
        [Required(ResourceName = "RequiredPaymentDate", ResourceSet = "Global")]

        public DateTime? TransactionDate { get; set; }

        [DataMember]
        [Required(ResourceName = "RequiredMessage")]

        public int? FundType { get; set; }

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
                error.Errors.Add(new ErrorData { ID = "RequiredFundDate" });
            }
            if (!FundType.HasValue)
            {
                error.Errors.Add(new ErrorData { ID = "RequiredFundType" });
            }

            if (!Amount.HasValue)
            {
                error.Errors.Add(new ErrorData { ID = "RequiredAmount" });
            }
            else
            {
                if (Amount <= 0 || Amount > 99999999)
                {
                    error.Errors.Add(new ErrorData { ID = "MaxFund" });
                }
            }
            if (!OriginalAmount.HasValue && CurrencyId.HasValue)
            {
                error.Errors.Add(new ErrorData { ID = "RequiredOriginalAmount" });
            }
            if (OriginalAmount.HasValue && !CurrencyId.HasValue)
            {
                error.Errors.Add(new ErrorData { ID = "RequiredCurrency" });
            }
            if (FundType.HasValue && (TypeId == (int)AccountFundTypeIds.ServiceCharge) && ((AccountFundTransTypeIds)FundType != AccountFundTransTypeIds.Cash))
            {
                error.Errors.Add(new ErrorData { ID = "ServiceChargeFundType" });
            }
            return error;
        }
    }

    [DataContract]
    public class FundDto
    {
        [DataMember]
        public decimal Amount { get; set; }
        [DataMember]
        public decimal VATAmount { get; set; }
        [DataMember]
        public DateTime TransactionDate { get; set; }
        [DataMember]
        public string ReceiptNo { get; set; }
        [DataMember]
        public string AdFalconReceiptNo { get; set; }

        [DataMember]
        public PaymentTypeDto Type { get; set; }
    }
}
