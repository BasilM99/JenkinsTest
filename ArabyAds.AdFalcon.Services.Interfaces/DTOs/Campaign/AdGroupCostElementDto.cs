using System;
using System.Collections.Generic;
using ProtoBuf;
using ArabyAds.AdFalcon.Domain.Common.Model.Campaign;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.AppSite;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Core;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Reports;
using ArabyAds.Framework.DataAnnotations;

using ArabyAds.Framework.Utilities;

namespace ArabyAds.AdFalcon.Services.Interfaces.DTOs.Campaign
{
    [ProtoContract]
    public class AdGroupCostElementSaveDto
    {
       [ProtoMember(1)]
        public int? ID { get; set; }
       [ProtoMember(2)]
        public virtual int CampaignId { get; set; }
       [ProtoMember(3)]
        public virtual int AdGroupId { get; set; }
       [ProtoMember(4)]
        public virtual int CostElementId { get; set; }
       [ProtoMember(5)]
        public virtual int? BeneficiaryId { get; set; }
       [ProtoMember(6)]
        public virtual int? ProviderId { get; set; }
       [ProtoMember(7)]
        [Required(ResourceName = "RequiredMessage")]
        [RegularExpression(@"^\$?\d+(\.(\d{1,2}))?$", ResourceName = "CurrencyMsg")]
        [Framework.DataAnnotations.Range(1, 100, ResourceName = "RangeMessage")]
        [System.ComponentModel.DataAnnotations.DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:##.##}")]
        public virtual decimal Value { get; set; }
       [ProtoMember(8)]
        public virtual DateTime FromDate { get; set; }
       [ProtoMember(9)]
        public virtual DateTime? ToDate { get; set; }
       [ProtoMember(10)]
        public virtual int? CostModelWrapperId { get; set; }
        [ProtoMember(11)]
        public virtual bool stop { get; set; }
        
    }

    [ProtoContract]
    public class AdGroupCostElementDto
    {
       [ProtoMember(1)]
        public int ID { get; set; }
       [ProtoMember(2)]
        [Required]
        public virtual string CostElement { get; set; }
       [ProtoMember(3)]
        public virtual string Beneficiary { get; set; }
       [ProtoMember(4)]
        [Required]
        public virtual int CostElementId { get; set; }
       [ProtoMember(5)]
        [Required]
        public virtual int ProviderId { get; set; }
       [ProtoMember(6)]
        public virtual string Provider { get; set; }
       [ProtoMember(7)]
        public virtual int? BeneficiaryId { get; set; }
       [ProtoMember(8)]
        [Required]
        [RegularExpression(@"^\$?\d+(\.(\d{1,2}))?$", ResourceName = "CurrencyMsg")]
        [System.ComponentModel.DataAnnotations.DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:##.##}")]
        public virtual decimal Value { get; set; }
       [ProtoMember(9)]
        public virtual DateTime FromDate { get; set; }
       [ProtoMember(10)]
        public virtual DateTime ToDate { get; set; }
       [ProtoMember(11)]
        public virtual string TypeName { get; set; }
        public virtual string ValueStr { get { return TypeName != null && TypeName.Equals("$") ? FormatHelper.FormatMoney(Value) : FormatHelper.FormatPercentage(Value / 100); } set { } }

       [ProtoMember(12)]
        public virtual bool IsOneTime { get; set; }

       [ProtoMember(13)]
        public virtual int Type { get; set; }
       [ProtoMember(14)]
        public virtual string EndDate {
            get {
                return Stoped ? ToDate.ToShortDateString() : ToDate == new DateTime(0001, 1, 1) { } ? "" : ToDate.ToShortDateString();
            }
            set { }
        }
       [ProtoMember(15)]
        public virtual bool Stoped
        {
            get
            {
                return ToDate <= Framework.Utilities.Environment.GetServerTime() && ToDate != new DateTime(0001, 1, 1) { };
            }
            set { }
        }
    }
    [ProtoContract]
    public class AdGroupCostElementResultDto
    {
       [ProtoMember(1)]
        public IEnumerable<AdGroupCostElementDto> Items { get; set; } = new List<AdGroupCostElementDto>();
        [ProtoMember(2)]
        public long TotalCount { get; set; }
    }



    [ProtoContract]
    public class AdGroupFeeSaveDto
    {
       [ProtoMember(1)]
        public int? ID { get; set; }
       [ProtoMember(2)]
        public virtual int CampaignId { get; set; }
       [ProtoMember(3)]
        public virtual int AdGroupId { get; set; }
       [ProtoMember(4)]
        public virtual int FeeId { get; set; }
       [ProtoMember(5)]
        public virtual int? BeneficiaryId { get; set; }
       [ProtoMember(6)]
        public virtual int? ProviderId { get; set; }
       [ProtoMember(7)]
        [Required(ResourceName = "RequiredMessage")]
        [RegularExpression(@"^\$?\d+(\.(\d{1,2}))?$", ResourceName = "CurrencyMsg")]
        [Framework.DataAnnotations.Range(1, 100, ResourceName = "RangeMessage")]
        [System.ComponentModel.DataAnnotations.DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:##.##}")]
        public virtual decimal Value { get; set; }
       [ProtoMember(8)]
        public virtual DateTime FromDate { get; set; }
       [ProtoMember(9)]
        public virtual DateTime? ToDate { get; set; }
       [ProtoMember(10)]
        public virtual int? CostModelWrapperId { get; set; }
    }

    [ProtoContract]
    public class AdGroupFeeDto
    {
       [ProtoMember(1)]
        public int ID { get; set; }
       [ProtoMember(2)]
       
        public virtual string Fee { get; set; }
       [ProtoMember(3)]
        public virtual string Beneficiary { get; set; }
       [ProtoMember(4)]
        
        public virtual int FeeId { get; set; }
       [ProtoMember(5)]
       
        public virtual int ProviderId { get; set; }
       [ProtoMember(6)]
        public virtual string Provider { get; set; }
       [ProtoMember(7)]
        public virtual int? BeneficiaryId { get; set; }
       [ProtoMember(8)]
        [Required]
      
        [RegularExpression(@"^\$?\d+(\.(\d{1,2}))?$", ResourceName = "CurrencyMsg")]
        [System.ComponentModel.DataAnnotations.DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:##.##}")]
        public virtual decimal Value { get; set; }
       [ProtoMember(9)]
        public virtual DateTime FromDate { get; set; }
       [ProtoMember(10)]
        public virtual DateTime ToDate { get; set; }
       [ProtoMember(11)]
        public virtual string TypeName { get; set; }
        public virtual string ValueStr { get { return TypeName != null && TypeName.Equals("$") ? FormatHelper.FormatMoney(Value) : FormatHelper.FormatPercentage(Value / 100); } set { } }

       [ProtoMember(12)]
        public virtual bool IsOneTime { get; set; }
       [ProtoMember(13)]
        public virtual bool IsRemoved { get; set; }
       [ProtoMember(14)]
        public virtual bool IsAdded { get; set; }
       [ProtoMember(15)]
        public virtual bool IsSystem { get; set; }
       [ProtoMember(16)]
        public virtual int Type { get; set; }
       [ProtoMember(17)]
        public virtual string EndDate
        {
            get
            {
                return Stoped ? ToDate.ToShortDateString() : ToDate == new DateTime(0001, 1, 1) { } ? "" : ToDate.ToShortDateString();
            }
            set { }
        }
       [ProtoMember(18)]
        public virtual bool Stoped
        {
            get
            {
                return ToDate <= Framework.Utilities.Environment.GetServerTime() && ToDate != new DateTime(0001, 1, 1) { };
            }
            set { }
        }
    }
    [ProtoContract]
    public class AdGroupFeeResultDto
    {
       [ProtoMember(1)]
        public IEnumerable<AdGroupFeeDto> Items { get; set; } = new List<AdGroupFeeDto>();
        [ProtoMember(2)]
        public long TotalCount { get; set; }
    }


    [ProtoContract]
    public class AdGroupDynamicBiddingConfigSaveDto
    {
       [ProtoMember(1)]
        public int? ID { get; set; }
       [ProtoMember(2)]
        public  int CampaignId { get; set; }
       [ProtoMember(3)]
        public  int AdGroupId { get; set; }
       [ProtoMember(4)]
        public  decimal BidOptimizationValue { get; set; }
       [ProtoMember(5)]
        public  decimal DefaultBidPrice { get; set; }
       [ProtoMember(6)]
        public  decimal MaxBidPrice { get; set; }
       [ProtoMember(7)]
        public  decimal MinBidPrice { get; set; }
       [ProtoMember(8)]
        public  decimal BidStep { get; set; }
       [ProtoMember(9)]
        public  bool KeepBiddingAtMinimum { get; set; }

       [ProtoMember(10)]
        public  BidOptimizationType Type { get; set; }
    }

    [ProtoContract]
    public class AdGroupDynamicBiddingConfigDto
    {
       [ProtoMember(1)]
        public int ID { get; set; }
      
        
   
      
       
        public  string ValueStr { get { return Type !=  BidOptimizationType.MaximizeCTR ? FormatHelper.FormatMoney(BidOptimizationValue) : FormatHelper.FormatPercentage(BidOptimizationValue); } set { } }
      
        public  string TypeStr { get { return Type.ToText(); } set { } }



       [ProtoMember(2)]


        public decimal BidOptimizationValue { get; set; }
       [ProtoMember(3)]
        public decimal DefaultBidPrice { get; set; }
       [ProtoMember(4)]
        public decimal MaxBidPrice { get; set; }
       [ProtoMember(5)]
        public decimal MinBidPrice { get; set; }
       [ProtoMember(6)]
        public decimal BidStep { get; set; }
       [ProtoMember(7)]
        public  bool KeepBiddingAtMinimum { get; set; }
        public string KeepBiddingAtMinimumStr { get {

                if (this.KeepBiddingAtMinimum)
                {
                    return "true";
                }

                return "false";

            } set { } }
       [ProtoMember(8)]
        public  BidOptimizationType Type { get; set; }
      
    }
    [ProtoContract]
    public class AdGroupDynamicBiddingConfigResultDto
    {
       [ProtoMember(1)]
        public IEnumerable<AdGroupDynamicBiddingConfigDto> Items { get; set; } = new List<AdGroupDynamicBiddingConfigDto>();
        [ProtoMember(2)]
        public long TotalCount { get; set; }
    }

}
