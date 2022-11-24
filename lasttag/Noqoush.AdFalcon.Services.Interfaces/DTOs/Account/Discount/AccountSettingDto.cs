using System;
using System.Runtime.Serialization;
using Noqoush.AdFalcon.Domain.Common.Model.Account;
using Noqoush.Framework.DataAnnotations;

namespace Noqoush.AdFalcon.Services.Interfaces.DTOs.Account.Discount
{
    [DataContract]
    public class AccountSettingDto
    {
        [DataMember]
        [Required(ResourceName = "RequiredMessage")]
        public virtual int AccountId { get; set; }

        [DataMember]
        //[Required(ResourceName = "RequiredMessage")]
        [RegularExpression(@"^\d+?$", ResourceName = "CurrencyMsg")]
        [Range(0, 100, ResourceName = "RangeMessage")]
        [System.ComponentModel.DataAnnotations.DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0}")]
        public float? Discount { get; set; }

        [DataMember]
        //[Required(ResourceName = "RequiredMessage")]
        [RegularExpression(@"^\d+?$", ResourceName = "CurrencyMsg")]
        [Framework.DataAnnotations.Range(1, 100, ResourceName = "RangeMessage")]
        [System.ComponentModel.DataAnnotations.DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0}")]
        public float? RevenuePercentage { get; set; }

        [DataMember]
        //[Required(ResourceName = "RequiredMessage")]
        [RegularExpression(@"^\$?\d+(\.(\d{1,2}))?$", ResourceName = "CurrencyMsg")]
        [Range(0, 1000000, ResourceName = "RangeMessage")]
        [System.ComponentModel.DataAnnotations.DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{F2")]
        public decimal OverDraft { get; set; }

        [DataMember]
        public bool AllowAPIAccess { get; set; }
        [DataMember]
        public AgencyCommission AgencyCommission { get; set; }



        [DataMember]
      
        public decimal AgencyCommissionValue { get; set; }
    }
}
