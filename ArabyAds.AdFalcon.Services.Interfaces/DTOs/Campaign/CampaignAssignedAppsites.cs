using ArabyAds.AdFalcon.Services.Interfaces.DTOs.AppSite;
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
    [ProtoInclude(100,typeof(CampaignAssignedAppsitesDto))]
    public class AssignedAppsitesDto
    {
        string subPublisherId;
       [ProtoMember(1)]
        public string ID { get; set; }
       [ProtoMember(2)]
        public string AppsiteName { get; set; }
       [ProtoMember(3)]
        public AppSiteBasicDto Appsite { get; set; }

   
       [ProtoMember(4)]
        public int? SubAppsiteId { get; set; }
       [ProtoMember(5)]
        public string SubPublisher { get { return subPublisherId; } set {; } }
       [ProtoMember(6)]
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
       [ProtoMember(7)]
        public bool Include { get; set; }


        [ProtoMember(8)]
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

        [ProtoMember(9)]
        public bool IsAdded { get; set; }
    }

    [ProtoContract]
    public class CampaignAssignedAppsitesSaveDTo
    {
       [ProtoMember(1)]
        public int CampaignId { get; set; }
       [ProtoMember(2)]
        public IEnumerable<CampaignAssignedAppsitesDto> InsertedItems { get; set; } = new List<CampaignAssignedAppsitesDto>();
       [ProtoMember(3)]
        public IEnumerable<CampaignBidConfigDto> NotCompatibleCampaignBidConfigs { get; set; } = new List<CampaignBidConfigDto>();

        [ProtoMember(4)]
        public IEnumerable<CampaignAssignedAppsitesDto> UpdatedItems { get; set; } = new List<CampaignAssignedAppsitesDto>();
        [ProtoMember(5)]
        public IList<int> DeletedAssignedAppsites { get; set; }
       [ProtoMember(6)]
        public long TotalCount { get; set; }
    }
    [ProtoContract]
    public class CampaignAssignedAppsitesModelDto
    {
        public CampaignAssignedAppsitesModelDto()
        {
            CampaignAssignedAppsitesList = new List<CampaignAssignedAppsitesDto>();
        }
       [ProtoMember(1)]
        public string CampignName { get; set; }
       [ProtoMember(2)]
        public IList<CampaignAssignedAppsitesDto> CampaignAssignedAppsitesList { get; set; }
    }
    [ProtoContract]
    public class CampaignAssignedAppsitesDto : AssignedAppsitesDto
    {

    }
}
