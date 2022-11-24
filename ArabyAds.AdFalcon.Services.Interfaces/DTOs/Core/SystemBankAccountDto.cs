using System;
using System.Collections.Generic;
using System.Text;
//using Gallio.Framework;
//using MbUnit.Framework;
//using MbUnit.Framework.ContractVerifiers;
using ProtoBuf;
using ArabyAds.Framework.DataAnnotations;

namespace ArabyAds.AdFalcon.Services.Interfaces.DTOs.Core
{
    [ProtoContract]
    public class SystemBankAccountDto
    {
       [ProtoMember(1)]
        [Required]
        public string BeneficiaryName
        {
            get;
            set;
        }

       [ProtoMember(2)]
        [Required]
        public string BankName
        {
            get;
            set;
        }

       [ProtoMember(3)]
        [Required]
        public string BankAddress
        {
            get;
            set;
        }

       [ProtoMember(4)]
        [Required]
        public string RecipientAccountNumber
        {
            get;
            set;
        }

       [ProtoMember(5)]
        [Required]
        [RegularExpression("[\\w]{8,11}", ResourceName = "InvalidSWIFT")]
        public string SWIFT
        {
            get;
            set;
        }
    }
}
