using System;
using System.Collections.Generic;
using ProtoBuf;
using ArabyAds.AdFalcon.Domain.Common.Model.Account;
using ArabyAds.AdFalcon.Domain.Common.Model.Campaign;
using ArabyAds.AdFalcon.Domain.Common.Model.Core;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.AppSite;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Core;
using ArabyAds.Framework.DataAnnotations;
using ArabyAds.Framework.ExceptionHandling.Exceptions;
using System.Text.Json.Serialization;

namespace ArabyAds.AdFalcon.Services.Interfaces.DTOs.Campaign
{
    [ProtoContract]
    public class CampaignSettingsDto
    {
       [ProtoMember(1)]
        public virtual int ID { get; set; }

         
       [ProtoMember(2)]
        public virtual bool ClientReadOnly { get; set; }
       

       [ProtoMember(3)]
        public virtual string Name { get; set; }

        public virtual string TrackConversionsUrlHttp { get; set; }

        public virtual string TrackConversionsUrlHttps { get; set; }
       [ProtoMember(4)]
        public virtual string UniqueId { get; set; }


       [ProtoMember(5)]
        public virtual bool TrackConversions { get; set; }

        public virtual bool TrackConversionsUseHttp { get; set; }

       [ProtoMember(6)]
        public virtual int PacingPoliciesValue { get; set; }

        public virtual bool TrackConversionsUseHttps { get; set; }
       [ProtoMember(7)]
        public virtual DiscountDto Discount { get; set; }
       [ProtoMember(8)]
        public virtual AdvertiserDto Advertiser { get; set; }
       [ProtoMember(9)]
        public int? CostModelWrapper { get; set; }

       [ProtoMember(10)]
        public bool IsClientLocked { get; set; }
       [ProtoMember(11)]
        public bool IsProgrammaticGuaranteed { get; set; }

        

       [ProtoMember(12)]
        public bool IsClientReadOnly { get; set; }

       [ProtoMember(13)]
        public string TrackingUrl { get; set; }

       [ProtoMember(14)]
        public bool LogAdMarkup { get; set; }
       [ProtoMember(15)]
        public PriceMode PriceMode { get; set; }
        public int PriceModeVal { get => (int)PriceMode; }
        [ProtoMember(16)]
        public CampaignLifeTime LifeTime { get; set; }
       [ProtoMember(17)]
        public AgencyCommission AgencyCommission { get; set; }


       [ProtoMember(18)]

        public decimal AgencyCommissionValue { get; set; }
       [ProtoMember(19)]
        //[Required(ResourceName = "RequiredMessage")]
        //[RegularExpression(@"^(https?:\/\/)?[a-zA-Z0-9]+(?:(?:\.|\-)[a-zA-Z0-9]+)+(?:\:\d+)?(?:\/[\w\-]+)*(?:\/?|\/\w+\.[a-zA-Z]{2,6}(?:\?[\w]+\=[\w\-]+)?)?(?:\&[\w]+\=[\w\-]+)*$", ResourceName = "UrlMsg")]
        public string DomainURL { get; set; }

       [ProtoMember(20)]
        public KeywordDto Keyword { get; set; }

        [RegularExpression(@"^\$?\d+(\.(\d{1,2}))?$", ResourceName = "CurrencyMsg")]
       [ProtoMember(21)]
        public decimal? CPMValue { get; set; }

       [ProtoMember(22)]
        public int? ValidCostModelWrapper { get; set; }

       [ProtoMember(23)]
        public virtual CampaignFrequencyCappingDto FrequencyCapping { get; set; }


       [ProtoMember(24)]
        public bool IsLocked { get; set; }

       [ProtoMember(25)]
        public virtual bool IsReadOnly
        {
            get; set;
        }

        [ProtoMember(26)]
        public virtual bool HasObjective
        {
            get; set;
        }
    }

    [ProtoContract]
    public class CampaignDto
    {


       [ProtoMember(1)]
        public virtual string AdvertiserAccountName { get; set; }

       [ProtoMember(2)]
        public virtual int AdvertiserAccountId { get; set; }

       [ProtoMember(3)]
        public virtual bool IsLocked { get; set; }
       [ProtoMember(4)]
        public virtual bool IsReadOnly
        {
            get; set;
        }

       [ProtoMember(5)]
        public virtual string UniqueId { get; set; }

       [ProtoMember(6)]
        public virtual AdvertiserDto Advertiser { get; set; }


       [ProtoMember(7)]
        public string AdvertiserName
        {
            get
            {

                if (Advertiser != null && Advertiser.Name != null)
                {
                    return Advertiser.Name.ToString();
                }
                return string.Empty;

            }
            set { }
        }


       [ProtoMember(8)]
        public int AdvertiserId
        {
            get
            {

                if (Advertiser != null)
                {
                    return Advertiser.ID;
                }
                return 0;

            }
            set { }
        }
       [ProtoMember(9)]
        public int ID { get; set; }

       [ProtoMember(10)]
        public bool TrackConversions { get; set; }

       [ProtoMember(11)]
        public virtual CampaignType CampaignType { get; set; }

       [ProtoMember(12)]
        [Required(ResourceName = "CampaignRequiredMsg")]
        [StringLength(255)]
        public string Name { get; set; }

       [ProtoMember(13)]
        [Required(ResourceName = "StartDateRequiredMsg")]
        public DateTime StartDate { get; set; }

       [ProtoMember(14)]
        public DateTime? EndDate { get; set; }

       [ProtoMember(15)]
        public DateTime? StartTime { get; set; }
       [ProtoMember(16)]
        public DateTime? EndTime { get; set; }

       [ProtoMember(17)]
        [Required(ResourceName = "BudgetRequiredMsg")]
        [RegularExpression(@"^\$?\d+(\.(\d{1,2}))?$", ResourceName = "CurrencyMsg")]
        [System.ComponentModel.DataAnnotations.DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:##.##}")]
        public decimal Budget { get; set; }

       [ProtoMember(18)]
        [RegularExpression(@"^\$?\d+(\.(\d{1,2}))?$", ResourceName = "CurrencyMsg")]
        [System.ComponentModel.DataAnnotations.DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:##.##}")]
        public decimal? DailyBudget { get; set; }

       [ProtoMember(19)]
        [StringLength(1024, ResourceName = "NoteLengthMsg")]
        public string Note { get; set; }

       [ProtoMember(20)]
        public int? CostModelWrapper { get; set; }

       [ProtoMember(21)]
        public bool IsRuntime { get; set; }

       [ProtoMember(22)]
        public string SupUserName { get; set; }

        [ProtoMember(23)]
        public List<AdGroupBidModifierDto> AdGroupBidModifiersDto { get; set; } = new List<AdGroupBidModifierDto>();

        [ProtoMember(24)]
        public CampaignSettingsDto CampaignSettingsDto { get; set; }

        [ProtoMember(25)]
        public bool HasObjective { get; set; }



    }

    [ProtoContract]
    public class CampaignSaveDto
    {
       [ProtoMember(1)]
        public virtual int ID { get; set; }
       [ProtoMember(2)]
        public virtual IList<ErrorData> Warnings { get; set; }

    }


    [ProtoContract]
    public class CampaignAllDto
    {
       [ProtoMember(1)]
        public CampaignSettingsDto oCampaignSettingsDto { get; set; }

       [ProtoMember(2)]
        public CampaignDto oCampaignDto { get; set; }
        [ProtoMember(3)]
        public IList<AdGroupBidModifierDto> AdGroupBidModifiersDto { get; set; }

    }

}
