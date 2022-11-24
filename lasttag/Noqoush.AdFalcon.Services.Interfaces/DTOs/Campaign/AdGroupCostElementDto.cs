using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Noqoush.AdFalcon.Domain.Common.Model.Campaign;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.AppSite;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Core;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Reports;
using Noqoush.Framework.DataAnnotations;

using Noqoush.Framework.Utilities;

namespace Noqoush.AdFalcon.Services.Interfaces.DTOs.Campaign
{
    [DataContract]
    public class AdGroupCostElementSaveDto
    {
        [DataMember]
        public int? ID { get; set; }
        [DataMember]
        public virtual int CampaignId { get; set; }
        [DataMember]
        public virtual int AdGroupId { get; set; }
        [DataMember]
        public virtual int CostElementId { get; set; }
        [DataMember]
        public virtual int? BeneficiaryId { get; set; }
        [DataMember]
        public virtual int? ProviderId { get; set; }
        [DataMember]
        [Required(ResourceName = "RequiredMessage")]
        [RegularExpression(@"^\$?\d+(\.(\d{1,2}))?$", ResourceName = "CurrencyMsg")]
        [Framework.DataAnnotations.Range(1, 100, ResourceName = "RangeMessage")]
        [System.ComponentModel.DataAnnotations.DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:##.##}")]
        public virtual decimal Value { get; set; }
        [DataMember]
        public virtual DateTime FromDate { get; set; }
        [DataMember]
        public virtual DateTime? ToDate { get; set; }
        [DataMember]
        public virtual int? CostModelWrapperId { get; set; }
    }

    [DataContract]
    public class AdGroupCostElementDto
    {
        [DataMember]
        public int ID { get; set; }
        [DataMember]
        [Required]
        public virtual string CostElement { get; set; }
        [DataMember]
        public virtual string Beneficiary { get; set; }
        [DataMember]
        [Required]
        public virtual int CostElementId { get; set; }
        [DataMember]
        [Required]
        public virtual int ProviderId { get; set; }
        [DataMember]
        public virtual string Provider { get; set; }
        [DataMember]
        public virtual int? BeneficiaryId { get; set; }
        [DataMember]
        [Required]
        [RegularExpression(@"^\$?\d+(\.(\d{1,2}))?$", ResourceName = "CurrencyMsg")]
        [System.ComponentModel.DataAnnotations.DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:##.##}")]
        public virtual decimal Value { get; set; }
        [DataMember]
        public virtual DateTime FromDate { get; set; }
        [DataMember]
        public virtual DateTime ToDate { get; set; }
        [DataMember]
        public virtual string TypeName { get; set; }
        public virtual string ValueStr { get { return TypeName != null && TypeName.Equals("$") ? FormatHelper.FormatMoney(Value) : FormatHelper.FormatPercentage(Value / 100); } set { } }

        [DataMember]
        public virtual bool IsOneTime { get; set; }

        [DataMember]
        public virtual int Type { get; set; }
        [DataMember]
        public virtual string EndDate {
            get {
                return Stoped ? ToDate.ToShortDateString() : ToDate == new DateTime(0001, 1, 1) { } ? "" : ToDate.ToShortDateString();
            }
            set { }
        }
        [DataMember]
        public virtual bool Stoped
        {
            get
            {
                return ToDate <= Framework.Utilities.Environment.GetServerTime() && ToDate != new DateTime(0001, 1, 1) { };
            }
            set { }
        }
    }
    [DataContract]
    public class AdGroupCostElementResultDto
    {
        [DataMember]
        public IEnumerable<AdGroupCostElementDto> Items { get; set; }
        [DataMember]
        public long TotalCount { get; set; }
    }



    [DataContract]
    public class AdGroupFeeSaveDto
    {
        [DataMember]
        public int? ID { get; set; }
        [DataMember]
        public virtual int CampaignId { get; set; }
        [DataMember]
        public virtual int AdGroupId { get; set; }
        [DataMember]
        public virtual int FeeId { get; set; }
        [DataMember]
        public virtual int? BeneficiaryId { get; set; }
        [DataMember]
        public virtual int? ProviderId { get; set; }
        [DataMember]
        [Required(ResourceName = "RequiredMessage")]
        [RegularExpression(@"^\$?\d+(\.(\d{1,2}))?$", ResourceName = "CurrencyMsg")]
        [Framework.DataAnnotations.Range(1, 100, ResourceName = "RangeMessage")]
        [System.ComponentModel.DataAnnotations.DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:##.##}")]
        public virtual decimal Value { get; set; }
        [DataMember]
        public virtual DateTime FromDate { get; set; }
        [DataMember]
        public virtual DateTime? ToDate { get; set; }
        [DataMember]
        public virtual int? CostModelWrapperId { get; set; }
    }

    [DataContract]
    public class AdGroupFeeDto
    {
        [DataMember]
        public int ID { get; set; }
        [DataMember]
       
        public virtual string Fee { get; set; }
        [DataMember]
        public virtual string Beneficiary { get; set; }
        [DataMember]
        
        public virtual int FeeId { get; set; }
        [DataMember]
       
        public virtual int ProviderId { get; set; }
        [DataMember]
        public virtual string Provider { get; set; }
        [DataMember]
        public virtual int? BeneficiaryId { get; set; }
        [DataMember]
        [Required]
      
        [RegularExpression(@"^\$?\d+(\.(\d{1,2}))?$", ResourceName = "CurrencyMsg")]
        [System.ComponentModel.DataAnnotations.DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:##.##}")]
        public virtual decimal Value { get; set; }
        [DataMember]
        public virtual DateTime FromDate { get; set; }
        [DataMember]
        public virtual DateTime ToDate { get; set; }
        [DataMember]
        public virtual string TypeName { get; set; }
        public virtual string ValueStr { get { return TypeName != null && TypeName.Equals("$") ? FormatHelper.FormatMoney(Value) : FormatHelper.FormatPercentage(Value / 100); } set { } }

        [DataMember]
        public virtual bool IsOneTime { get; set; }
        [DataMember]
        public virtual bool IsRemoved { get; set; }
        [DataMember]
        public virtual bool IsAdded { get; set; }
        [DataMember]
        public virtual bool IsSystem { get; set; }
        [DataMember]
        public virtual int Type { get; set; }
        [DataMember]
        public virtual string EndDate
        {
            get
            {
                return Stoped ? ToDate.ToShortDateString() : ToDate == new DateTime(0001, 1, 1) { } ? "" : ToDate.ToShortDateString();
            }
            set { }
        }
        [DataMember]
        public virtual bool Stoped
        {
            get
            {
                return ToDate <= Framework.Utilities.Environment.GetServerTime() && ToDate != new DateTime(0001, 1, 1) { };
            }
            set { }
        }
    }
    [DataContract]
    public class AdGroupFeeResultDto
    {
        [DataMember]
        public IEnumerable<AdGroupFeeDto> Items { get; set; }
        [DataMember]
        public long TotalCount { get; set; }
    }


    [DataContract]
    public class AdGroupDynamicBiddingConfigSaveDto
    {
        [DataMember]
        public int? ID { get; set; }
        [DataMember]
        public  int CampaignId { get; set; }
        [DataMember]
        public  int AdGroupId { get; set; }
        [DataMember]
        public  decimal BidOptimizationValue { get; set; }
        [DataMember]
        public  decimal DefaultBidPrice { get; set; }
        [DataMember]
        public  decimal MaxBidPrice { get; set; }
        [DataMember]
        public  decimal MinBidPrice { get; set; }
        [DataMember]
        public  decimal BidStep { get; set; }
        [DataMember]
        public  bool KeepBiddingAtMinimum { get; set; }

        [DataMember]
        public  BidOptimizationType Type { get; set; }
    }

    [DataContract]
    public class AdGroupDynamicBiddingConfigDto
    {
        [DataMember]
        public int ID { get; set; }
      
        
   
      
       
        public  string ValueStr { get { return Type !=  BidOptimizationType.MaximizeCTR ? FormatHelper.FormatMoney(BidOptimizationValue) : FormatHelper.FormatPercentage(BidOptimizationValue); } set { } }
      
        public  string TypeStr { get { return Type.ToText(); } set { } }



        [DataMember]


        public decimal BidOptimizationValue { get; set; }
        [DataMember]
        public decimal DefaultBidPrice { get; set; }
        [DataMember]
        public decimal MaxBidPrice { get; set; }
        [DataMember]
        public decimal MinBidPrice { get; set; }
        [DataMember]
        public decimal BidStep { get; set; }
        [DataMember]
        public  bool KeepBiddingAtMinimum { get; set; }
        public string KeepBiddingAtMinimumStr { get {

                if (this.KeepBiddingAtMinimum)
                {
                    return "true";
                }

                return "false";

            } set { } }
        [DataMember]
        public  BidOptimizationType Type { get; set; }
      
    }
    [DataContract]
    public class AdGroupDynamicBiddingConfigResultDto
    {
        [DataMember]
        public IEnumerable<AdGroupDynamicBiddingConfigDto> Items { get; set; }
        [DataMember]
        public long TotalCount { get; set; }
    }

}
