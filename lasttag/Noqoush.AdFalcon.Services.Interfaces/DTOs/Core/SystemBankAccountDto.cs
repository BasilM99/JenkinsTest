using System;
using System.Collections.Generic;
using System.Text;
using Gallio.Framework;
using MbUnit.Framework;
using MbUnit.Framework.ContractVerifiers;
using System.Runtime.Serialization;
using Noqoush.Framework.DataAnnotations;

namespace Noqoush.AdFalcon.Services.Interfaces.DTOs.Core
{
    [DataContract]
    public class SystemBankAccountDto
    {
        [DataMember]
        [Required]
        public string BeneficiaryName
        {
            get;
            set;
        }

        [DataMember]
        [Required]
        public string BankName
        {
            get;
            set;
        }

        [DataMember]
        [Required]
        public string BankAddress
        {
            get;
            set;
        }

        [DataMember]
        [Required]
        public string RecipientAccountNumber
        {
            get;
            set;
        }

        [DataMember]
        [Required]
        [RegularExpression("[\\w]{8,11}", ResourceName = "InvalidSWIFT")]
        public string SWIFT
        {
            get;
            set;
        }
    }
}
