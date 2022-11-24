using ArabyAds.AdFalcon.Services.Interfaces.DTOs.AppSite;
using ArabyAds.Framework.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using ProtoBuf;
using System.Text;
using System.Threading.Tasks;
using ArabyAds.Framework.Resources;

namespace ArabyAds.AdFalcon.Services.Interfaces.DTOs.Campaign
{
    [ProtoContract]
    public class CampaignBidConfigSaveDTo
    {
       [ProtoMember(1)]
        public int CampaignId { get; set; }

       [ProtoMember(2)]
        public int AdGroupId { get; set; }

       [ProtoMember(3)]
        public IEnumerable<CampaignBidConfigDto> InsertedItems { get; set; }
       [ProtoMember(4)]
        public IEnumerable<CampaignBidConfigDto> UpdatedItems { get; set; }
       [ProtoMember(5)]
        public IList<int> DeletedCampaignBidConfigs { get; set; }
       [ProtoMember(6)]
        public long TotalCount { get; set; }
    }

    [ProtoContract]
    public class CampaignBidConfigModelDto
    {
        public CampaignBidConfigModelDto()
        {
            CampaignBidConfigDtos = new List<CampaignBidConfigDto>();
        }
       [ProtoMember(1)]
        public string CampignName { get; set; }
       [ProtoMember(2)]
        public IList<CampaignBidConfigDto> CampaignBidConfigDtos { get; set; }
    }

    [ProtoContract]
    public class CampaignBidConfigDto
    {
        string subPublisherId;

        int appSiteId;
        [ProtoMember(1)]
        public string ID { get; set; }
       [ProtoMember(2)]
        public int AccountId { get; set; }
       [ProtoMember(3)]
        public int AdGroupId { get; set; }
       [ProtoMember(4)]
        public string AdGroupPricingModel { get; set; }
       [ProtoMember(5)]
        public string AdGrouptName { get; set; }
       [ProtoMember(6)]
        public string CampaingName { get; set; }
       [ProtoMember(7)]
        public string AccountName { get; set; }
       [ProtoMember(8)]
        public string AppsiteName { get; set; }
       [ProtoMember(9)]
        public AppSiteBasicDto Appsite { get; set; }
       [ProtoMember(10)]
        public string AppsitePricingModel { get; set; }
       [ProtoMember(11)]
        public int AppsitePricingModelId { get; set; }
       [ProtoMember(12)]
        public string SubPublisher { get { return subPublisherId; } set { ;} }
       [ProtoMember(13)]
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
       [ProtoMember(14)]
        [RegularExpression(@"^\$?\d+(\.(\d{1,2}))?$", ResourceName = "CurrencyMsg")]
        [Required()]
        public decimal Bid { get; set; }
       [ProtoMember(15)]
        public decimal MinBid { get; set; }

       [ProtoMember(16)]
        public bool HideDeleteButton { get; set; }

       [ProtoMember(17)]
        public int? SubAppsiteId { get; set; }
        [ProtoMember(18)]
        public string AppsitePricingModelString  { get {


                if (string.IsNullOrWhiteSpace(this.AppsitePricingModel))
                   {
                    return ResourceManager.Instance.GetResource("Default", "Campaign");
                  }
                    else
                { 
                    this.AppsitePricingModel.ToString(); 
                
                }
                return string.Empty;

            } set { } }


        [ProtoMember(19)]
        public string SubPublisherString
        {
            get
            {
               // (string.IsNullOrEmpty(r.SubPublisher) || r.SubPublisher == null) ? Html.GetResource("All", "CampaignAssignAppsites")

                if (this.SubPublisher == string.Empty)
                {
                    return ResourceManager.Instance.GetResource("All", "CampaignAssignAppsites");
                }
                else
                {
                    return this.SubPublisher;

                }
                return string.Empty;

            }
            set { }
        }

        [ProtoMember(20)]
        public bool IsAdded
        {
            get;
            set;
        }
        [ProtoMember(21)]
        public string BidString { get { return Bid.ToString(); } set { } }

   

        [ProtoMember(22)]
        public int AppSiteId { get {

                if (this.Appsite!=null && this.Appsite.ID>0) 
                    appSiteId= this.Appsite.ID;
                return appSiteId;
            } set { appSiteId = value; } }
    }
}
