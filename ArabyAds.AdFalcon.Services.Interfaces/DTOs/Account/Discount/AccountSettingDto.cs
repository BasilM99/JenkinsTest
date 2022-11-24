using System;
using ProtoBuf;
using ArabyAds.AdFalcon.Domain.Common.Model.Account;
using ArabyAds.Framework.DataAnnotations;

namespace ArabyAds.AdFalcon.Services.Interfaces.DTOs.Account.Discount
{
    [ProtoContract]
    public class AccountSettingDto
    {
       [ProtoMember(1)]
        [Required(ResourceName = "RequiredMessage")]
        public virtual int AccountId { get; set; }

       [ProtoMember(2)]
        //[Required(ResourceName = "RequiredMessage")]
        [RegularExpression(@"^\d+?$", ResourceName = "CurrencyMsg")]
        [Range(0, 100, ResourceName = "RangeMessage")]
        [System.ComponentModel.DataAnnotations.DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0}")]
        public float? Discount { get; set; }

       [ProtoMember(3)]
        //[Required(ResourceName = "RequiredMessage")]
        [RegularExpression(@"^\d+?$", ResourceName = "CurrencyMsg")]
        [Framework.DataAnnotations.Range(1, 100, ResourceName = "RangeMessage")]
        [System.ComponentModel.DataAnnotations.DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0}")]
        public float? RevenuePercentage { get; set; }

       [ProtoMember(4)]
        //[Required(ResourceName = "RequiredMessage")]
        [RegularExpression(@"^\$?\d+(\.(\d{1,2}))?$", ResourceName = "CurrencyMsg")]
        [Range(0, 1000000, ResourceName = "RangeMessage")]
        [System.ComponentModel.DataAnnotations.DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{F2")]
        public decimal OverDraft { get; set; }

       [ProtoMember(5)]
        public bool AllowAPIAccess { get; set; }
       [ProtoMember(6)]
        public AgencyCommission AgencyCommission { get; set; }



       [ProtoMember(7)]
      
        public decimal AgencyCommissionValue { get; set; }
    }
}
