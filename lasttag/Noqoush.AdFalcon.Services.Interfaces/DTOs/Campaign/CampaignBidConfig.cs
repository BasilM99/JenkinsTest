using Noqoush.AdFalcon.Services.Interfaces.DTOs.AppSite;
using Noqoush.Framework.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Noqoush.AdFalcon.Services.Interfaces.DTOs.Campaign
{
    [DataContract]
    public class CampaignBidConfigSaveDTo
    {
        [DataMember]
        public int CampaignId { get; set; }

        [DataMember]
        public int AdGroupId { get; set; }

        [DataMember]
        public IEnumerable<CampaignBidConfigDto> InsertedItems { get; set; }
        [DataMember]
        public IEnumerable<CampaignBidConfigDto> UpdatedItems { get; set; }
        [DataMember]
        public IList<int> DeletedCampaignBidConfigs { get; set; }
        [DataMember]
        public long TotalCount { get; set; }
    }

    [DataContract]
    public class CampaignBidConfigModelDto
    {
        public CampaignBidConfigModelDto()
        {
            CampaignBidConfigDtos = new List<CampaignBidConfigDto>();
        }
        [DataMember]
        public string CampignName { get; set; }
        [DataMember]
        public IList<CampaignBidConfigDto> CampaignBidConfigDtos { get; set; }
    }

    [DataContract]
    public class CampaignBidConfigDto
    {
        string subPublisherId;
        [DataMember]
        public string ID { get; set; }
        [DataMember]
        public int AccountId { get; set; }
        [DataMember]
        public int AdGroupId { get; set; }
        [DataMember]
        public string AdGroupPricingModel { get; set; }
        [DataMember]
        public string AdGrouptName { get; set; }
        [DataMember]
        public string CampaingName { get; set; }
        [DataMember]
        public string AccountName { get; set; }
        [DataMember]
        public string AppsiteName { get; set; }
        [DataMember]
        public AppSiteBasicDto Appsite { get; set; }
        [DataMember]
        public string AppsitePricingModel { get; set; }
        [DataMember]
        public int AppsitePricingModelId { get; set; }
        [DataMember]
        public string SubPublisher { get { return subPublisherId; } set { ;} }
        [DataMember]
        public string SubPublisherId
        {
            get
            {
                if (string.IsNullOrEmpty(subPublisherId))
                    return null;
                return subPublisherId;
            }
            set
            { subPublisherId = value; }
        }
        [DataMember]
        [RegularExpression(@"^\$?\d+(\.(\d{1,2}))?$", ResourceName = "CurrencyMsg")]
        [Required()]
        public decimal Bid { get; set; }
        [DataMember]
        public decimal MinBid { get; set; }

        [DataMember]
        public bool HideDeleteButton { get; set; }

        [DataMember]
        public int? SubAppsiteId { get; set; }

    }
}
