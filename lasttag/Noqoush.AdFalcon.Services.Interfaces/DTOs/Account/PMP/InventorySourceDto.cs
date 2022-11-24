using Noqoush.AdFalcon.Services.Interfaces.DTOs.AppSite;
using Noqoush.Framework.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
namespace Noqoush.AdFalcon.Services.Interfaces.DTOs.Account.PMP
{
   
    [DataContract]
    public class InventorySourceSaveDTo
    {
        [DataMember]
        public int CampaignId { get; set; }

        [DataMember]
        public int AdGroupId { get; set; }

        [DataMember]
        public IEnumerable<InventorySourceDto> InsertedItems { get; set; }
        [DataMember]
        public IEnumerable<InventorySourceDto> UpdatedItems { get; set; }
        [DataMember]
        public IList<int> DeletedInventorySources { get; set; }
        [DataMember]
        public long TotalCount { get; set; }
    }

    [DataContract]
    public class InventorySourceModelDto
    {
        public InventorySourceModelDto()
        {
            InventorySourceDtos = new List<InventorySourceDto>();
        }
        [DataMember]
        public string CampignName { get; set; }
        [DataMember]
        public IList<InventorySourceDto> InventorySourceDtos { get; set; }
    }

    [DataContract]
    public class InventorySourceDto
    {
        [DataMember]
        public int ID { get; set; }
        [DataMember]
        public int SubAppSiteId { get; set; }
        [DataMember]
        public int SSPId { get; set; }
        [DataMember]
        public string subPublisherId { get; set; }

        [DataMember]
        public SubAppsiteDto subPublisherDto { get; set; }
      
        [DataMember]
        public int AccountId { get; set; }
        [DataMember]
        public int AdGroupId { get; set; }

        [DataMember]
        public string AdGrouptName { get; set; }
        [DataMember]
        public string CampaingName { get; set; }
        [DataMember]
        public string ExchangeName { get; set; }
        [DataMember]
        public string SubPublisherMarketId { get; set; }


        [DataMember]
        public string AppsiteName { get; set; }
        [DataMember]
        public AppSiteBasicDto Appsite { get; set; }
        [DataMember]
        public string AppsitePricingModel { get; set; }
        [DataMember]
        public int AppsitePricingModelId { get; set; }
        [DataMember]
        public string SubPublisher { get; set; }


        [DataMember]
        public bool Include { get; set; }

    }
}
