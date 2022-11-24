using ArabyAds.AdFalcon.Services.Interfaces.DTOs.AppSite;
using ArabyAds.Framework.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using ProtoBuf;
using System.Text;
using System.Threading.Tasks;
namespace ArabyAds.AdFalcon.Services.Interfaces.DTOs.Account.PMP
{
   
    [ProtoContract]
    public class InventorySourceSaveDTo
    {
       [ProtoMember(1)]
        public int CampaignId { get; set; }

       [ProtoMember(2)]
        public int AdGroupId { get; set; }

       [ProtoMember(3)]
        public IEnumerable<InventorySourceDto> InsertedItems { get; set; }
       [ProtoMember(4)]
        public IEnumerable<InventorySourceDto> UpdatedItems { get; set; }
       [ProtoMember(5)]
        public IList<int> DeletedInventorySources { get; set; }
       [ProtoMember(6)]
        public long TotalCount { get; set; }
    }

    [ProtoContract]
    public class InventorySourceModelDto
    {
        public InventorySourceModelDto()
        {
            InventorySourceDtos = new List<InventorySourceDto>();
        }
       [ProtoMember(1)]
        public string CampignName { get; set; }
       [ProtoMember(2)]
        public IList<InventorySourceDto> InventorySourceDtos { get; set; }
    }

    [ProtoContract]
    public class InventorySourceDto
    {
       [ProtoMember(1)]
        public int ID { get; set; }
       [ProtoMember(2)]
        public int SubAppSiteId { get; set; }
       [ProtoMember(3)]
        public int SSPId { get; set; }
       [ProtoMember(4)]
        public string subPublisherId { get; set; }

       [ProtoMember(5)]
        public SubAppsiteDto subPublisherDto { get; set; }
      
       [ProtoMember(6)]
        public int AccountId { get; set; }
       [ProtoMember(7)]
        public int AdGroupId { get; set; }

       [ProtoMember(8)]
        public string AdGrouptName { get; set; }
       [ProtoMember(9)]
        public string CampaingName { get; set; }
       [ProtoMember(10)]
        public string ExchangeName { get; set; }
       [ProtoMember(11)]
        public string SubPublisherMarketId { get; set; }


       [ProtoMember(12)]
        public string AppsiteName { get; set; }
       [ProtoMember(13)]
        public AppSiteBasicDto Appsite { get; set; }
       [ProtoMember(14)]
        public string AppsitePricingModel { get; set; }
       [ProtoMember(15)]
        public int AppsitePricingModelId { get; set; }
       [ProtoMember(16)]
        public string SubPublisher { get; set; }


       [ProtoMember(17)]
        public bool Include { get; set; }

    }
}
