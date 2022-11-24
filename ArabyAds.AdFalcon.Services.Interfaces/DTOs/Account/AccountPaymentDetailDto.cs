using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProtoBuf;
using ArabyAds.Framework.DataAnnotations;
using System.Web;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Core;

namespace ArabyAds.AdFalcon.Services.Interfaces.DTOs.Account
{
    /*[ProtoContract]
    public class AccountPaymentDetailDtoBase
    {
        /// <summary>
        /// 1-bank account
        /// 2-paypal account
        /// </summary>
       [ProtoMember()]
        //[Required]
        public int TypeId { get; set; }

       [ProtoMember()]
        //[Required]
        public string BeneficiaryName
        {
            get;
            set;
        }

       [ProtoMember()]
        //[Required]
        public string BankName
        {
            get;
            set;
        }

       [ProtoMember()]
        public string UserName
        {
            get;
            set;
        }

       [ProtoMember()]
        //[Required]
        public string BankAddress
        {
            get;
            set;
        }

       [ProtoMember()]
        //[Required]
        public string RecipientAccountNumber
        {
            get;
            set;
        }
    }
    */
    [ProtoContract]
    public class AccountPaymentDetailDto
    {
        /// <summary>
        /// 1-bank account
        /// 2-paypal account
        /// </summary>
       [ProtoMember(1)]
        [Required]
        public int TypeId { get; set; }

       [ProtoMember(2)]
        //[Required]
        public string BeneficiaryName
        {
            get;
            set;
        }

       [ProtoMember(3)]
        //[Required]
        public string BankName
        {
            get;
            set;
        }

       [ProtoMember(4)]
        public string UserName
        {
            get;
            set;
        }

       [ProtoMember(5)]

        public string TaxNumberRegex
        {
            get;
            set;
        }

       [ProtoMember(6)]
        //[RegularExpression(gettest, ResourceName = "InvalidSWIFT")]
        public string TaxNumber
        {
            get;
            set;
        }

       [ProtoMember(7)]
        public byte[] TaxDocumentBytes { get; set; }

       [ProtoMember(8)]
        public DocumentDto Document { get; set; }

       [ProtoMember(9)]
        //[Required]
        public string BankAddress
        {
            get;
            set;
        }

       [ProtoMember(10)]
        //[Required]
        public string RecipientAccountNumber
        {
            get;
            set;
        }

       [ProtoMember(11)]
        //[Required]
        [RegularExpression("[\\w]{8,11}", ResourceName = "InvalidSWIFT")]
        public string SWIFT
        {
            get;
            set;
        }
    }

    [ProtoContract]
    public class AccountPaymentDetailFundDto
    {
        /// <summary>
        /// 1-bank account
        /// 2-paypal account
        /// </summary>
       [ProtoMember(1)]
        public int TypeId { get; set; }

       [ProtoMember(2)]
        //[Required(ResourceName = "RequiredMessage")]

        public string BeneficiaryName
        {
            get;
            set;
        }

        
       [ProtoMember(3)]
       // [Required(ResourceName = "RequiredMessage")]

        public string BankName
        {
            get;
            set;
        }

       [ProtoMember(4)]
       // [Required(ResourceName = "RequiredMessage")]

        public string UserName
        {
            get;
            set;
        }

       [ProtoMember(5)]
       //    [Required(ResourceName = "RequiredMessage")]

        public string BankAddress
        {
            get;
            set;
        }

       [ProtoMember(6)]
       // [Required(ResourceName = "RequiredMessage")]

        public string RecipientAccountNumber
        {
            get;
            set;
        }
       [ProtoMember(7)]
       // [Required(ResourceName = "RequiredMessage")]

        public string SWIFT
        {
            get;
            set;
        }
    }
}
