using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Noqoush.AdFalcon.Domain.Common.Model.Account;
using Noqoush.AdFalcon.Domain.Common.Model.Campaign;
using Noqoush.AdFalcon.Domain.Common.Model.Core;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.AppSite;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Core;
using Noqoush.Framework.DataAnnotations;
using Noqoush.Framework.ExceptionHandling.Exceptions;

namespace Noqoush.AdFalcon.Services.Interfaces.DTOs.Campaign
{
    [DataContract]
    public class CampaignSettingsDto
    {
        [DataMember]
        public virtual int ID { get; set; }

         
        [DataMember]
        public virtual bool ClientReadOnly { get; set; }
       

        [DataMember]
        public virtual string Name { get; set; }

        public virtual string TrackConversionsUrlHttp { get; set; }

        public virtual string TrackConversionsUrlHttps { get; set; }
        [DataMember]
        public virtual string UniqueId { get; set; }


        [DataMember]
        public virtual bool TrackConversions { get; set; }

        public virtual bool TrackConversionsUseHttp { get; set; }

        [DataMember]
        public virtual int PacingPoliciesValue { get; set; }

        public virtual bool TrackConversionsUseHttps { get; set; }
        [DataMember]
        public virtual DiscountDto Discount { get; set; }
        [DataMember]
        public virtual AdvertiserDto Advertiser { get; set; }
        [DataMember]
        public int? CostModelWrapper { get; set; }

        [DataMember]
        public bool IsClientLocked { get; set; }
        [DataMember]
        public bool IsProgrammaticGuaranteed { get; set; }

        

        [DataMember]
        public bool IsClientReadOnly { get; set; }

        [DataMember]
        public string TrackingUrl { get; set; }

        [DataMember]
        public bool LogAdMarkup { get; set; }
        [DataMember]
        public PriceMode PriceMode { get; set; }
        [DataMember]
        public CampaignLifeTime LifeTime { get; set; }
        [DataMember]
        public AgencyCommission AgencyCommission { get; set; }


        [DataMember]

        public decimal AgencyCommissionValue { get; set; }
        [DataMember]
        //[Required(ResourceName = "RequiredMessage")]
        //[RegularExpression(@"^(https?:\/\/)?[a-zA-Z0-9]+(?:(?:\.|\-)[a-zA-Z0-9]+)+(?:\:\d+)?(?:\/[\w\-]+)*(?:\/?|\/\w+\.[a-zA-Z]{2,6}(?:\?[\w]+\=[\w\-]+)?)?(?:\&[\w]+\=[\w\-]+)*$", ResourceName = "UrlMsg")]
        public string DomainURL { get; set; }

        [DataMember]
        public KeywordDto Keyword { get; set; }

        [RegularExpression(@"^\$?\d+(\.(\d{1,2}))?$", ResourceName = "CurrencyMsg")]
        [DataMember]
        public decimal? CPMValue { get; set; }

        [DataMember]
        public int? ValidCostModelWrapper { get; set; }

        [DataMember]
        public virtual CampaignFrequencyCappingDto FrequencyCapping { get; set; }


        [DataMember]
        public bool IsLocked { get; set; }

        [DataMember]
        public virtual bool IsReadOnly
        {
            get; set;
        }
    }

    [DataContract]
    public class CampaignDto
    {


        [DataMember]
        public virtual string AdvertiserAccountName { get; set; }

        [DataMember]
        public virtual int AdvertiserAccountId { get; set; }

        [DataMember]
        public virtual bool IsLocked { get; set; }
        [DataMember]
        public virtual bool IsReadOnly
        {
            get; set;
        }

        [DataMember]
        public virtual string UniqueId { get; set; }

        [DataMember]
        public virtual AdvertiserDto Advertiser { get; set; }


        [DataMember]
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


        [DataMember]
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
        [DataMember]
        public int ID { get; set; }

        [DataMember]
        public bool TrackConversions { get; set; }

        [DataMember]
        public virtual CampaignType CampaignType { get; set; }

        [DataMember]
        [Required(ResourceName = "CampaignRequiredMsg")]
        [StringLength(255)]
        public string Name { get; set; }

        [DataMember]
        [Required(ResourceName = "StartDateRequiredMsg")]
        public DateTime StartDate { get; set; }

        [DataMember]
        public DateTime? EndDate { get; set; }

        [DataMember]
        public DateTime? StartTime { get; set; }
        [DataMember]
        public DateTime? EndTime { get; set; }

        [DataMember]
        [Required(ResourceName = "BudgetRequiredMsg")]
        [RegularExpression(@"^\$?\d+(\.(\d{1,2}))?$", ResourceName = "CurrencyMsg")]
        [System.ComponentModel.DataAnnotations.DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:##.##}")]
        public decimal Budget { get; set; }

        [DataMember]
        [RegularExpression(@"^\$?\d+(\.(\d{1,2}))?$", ResourceName = "CurrencyMsg")]
        [System.ComponentModel.DataAnnotations.DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:##.##}")]
        public decimal? DailyBudget { get; set; }

        [DataMember]
        [StringLength(1024, ResourceName = "NoteLengthMsg")]
        public string Note { get; set; }

        [DataMember]
        public int? CostModelWrapper { get; set; }

        [DataMember]
        public bool IsRuntime { get; set; }

        [DataMember]
        public string SupUserName { get; set; }
    }

    [DataContract]
    public class CampaignSaveDto
    {
        [DataMember]
        public virtual int ID { get; set; }
        [DataMember]
        public virtual IList<ErrorData> Warnings { get; set; }

    }


    [DataContract]
    public class CampaignAllDto
    {
        [DataMember]
        public CampaignSettingsDto oCampaignSettingsDto { get; set; }

        [DataMember]
        public CampaignDto oCampaignDto { get; set; }

    }

}
