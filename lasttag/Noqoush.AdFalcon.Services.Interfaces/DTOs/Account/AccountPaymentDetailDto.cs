using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Noqoush.Framework.DataAnnotations;
using System.Web;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Core;

namespace Noqoush.AdFalcon.Services.Interfaces.DTOs.Account
{
    /*[DataContract]
    public class AccountPaymentDetailDtoBase
    {
        /// <summary>
        /// 1-bank account
        /// 2-paypal account
        /// </summary>
        [DataMember]
        //[Required]
        public int TypeId { get; set; }

        [DataMember]
        //[Required]
        public string BeneficiaryName
        {
            get;
            set;
        }

        [DataMember]
        //[Required]
        public string BankName
        {
            get;
            set;
        }

        [DataMember]
        public string UserName
        {
            get;
            set;
        }

        [DataMember]
        //[Required]
        public string BankAddress
        {
            get;
            set;
        }

        [DataMember]
        //[Required]
        public string RecipientAccountNumber
        {
            get;
            set;
        }
    }
    */
    [DataContract]
    public class AccountPaymentDetailDto
    {
        /// <summary>
        /// 1-bank account
        /// 2-paypal account
        /// </summary>
        [DataMember]
        [Required]
        public int TypeId { get; set; }

        [DataMember]
        //[Required]
        public string BeneficiaryName
        {
            get;
            set;
        }

        [DataMember]
        //[Required]
        public string BankName
        {
            get;
            set;
        }

        [DataMember]
        public string UserName
        {
            get;
            set;
        }

        [DataMember]

        public string TaxNumberRegex
        {
            get;
            set;
        }

        [DataMember]
        //[RegularExpression(gettest, ResourceName = "InvalidSWIFT")]
        public string TaxNumber
        {
            get;
            set;
        }

        [DataMember]
        public byte[] TaxDocumentBytes { get; set; }

        [DataMember]
        public DocumentDto Document { get; set; }

        [DataMember]
        //[Required]
        public string BankAddress
        {
            get;
            set;
        }

        [DataMember]
        //[Required]
        public string RecipientAccountNumber
        {
            get;
            set;
        }

        [DataMember]
        //[Required]
        [RegularExpression("[\\w]{8,11}", ResourceName = "InvalidSWIFT")]
        public string SWIFT
        {
            get;
            set;
        }
    }

    [DataContract]
    public class AccountPaymentDetailFundDto
    {
        /// <summary>
        /// 1-bank account
        /// 2-paypal account
        /// </summary>
        [DataMember]
        public int TypeId { get; set; }

        [DataMember]
        //[Required(ResourceName = "RequiredMessage")]

        public string BeneficiaryName
        {
            get;
            set;
        }

        
        [DataMember]
       // [Required(ResourceName = "RequiredMessage")]

        public string BankName
        {
            get;
            set;
        }

        [DataMember]
       // [Required(ResourceName = "RequiredMessage")]

        public string UserName
        {
            get;
            set;
        }

        [DataMember]
       //    [Required(ResourceName = "RequiredMessage")]

        public string BankAddress
        {
            get;
            set;
        }

        [DataMember]
       // [Required(ResourceName = "RequiredMessage")]

        public string RecipientAccountNumber
        {
            get;
            set;
        }
        [DataMember]
       // [Required(ResourceName = "RequiredMessage")]

        public string SWIFT
        {
            get;
            set;
        }
    }
}
