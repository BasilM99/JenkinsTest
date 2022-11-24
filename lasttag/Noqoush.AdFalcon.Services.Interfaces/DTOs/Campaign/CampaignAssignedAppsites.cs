using Noqoush.AdFalcon.Services.Interfaces.DTOs.AppSite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Noqoush.AdFalcon.Services.Interfaces.DTOs.Campaign
{
    [DataContract]
    public class AssignedAppsitesDto
    {
        string subPublisherId;
        [DataMember]
        public string ID { get; set; }
        [DataMember]
        public string AppsiteName { get; set; }
        [DataMember]
        public AppSiteBasicDto Appsite { get; set; }

   
        [DataMember]
        public int? SubAppsiteId { get; set; }
        [DataMember]
        public string SubPublisher { get { return subPublisherId; } set {; } }
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
        public bool Include { get; set; }
    }

    [DataContract]
    public class CampaignAssignedAppsitesSaveDTo
    {
        [DataMember]
        public int CampaignId { get; set; }
        [DataMember]
        public IEnumerable<CampaignAssignedAppsitesDto> InsertedItems { get; set; }
        [DataMember]
        public IEnumerable<CampaignBidConfigDto> NotCompatibleCampaignBidConfigs { get; set; }

        [DataMember]
        public IEnumerable<CampaignAssignedAppsitesDto> UpdatedItems { get; set; }
        [DataMember]
        public IList<int> DeletedAssignedAppsites { get; set; }
        [DataMember]
        public long TotalCount { get; set; }
    }
    [DataContract]
    public class CampaignAssignedAppsitesModelDto
    {
        public CampaignAssignedAppsitesModelDto()
        {
            CampaignAssignedAppsitesList = new List<CampaignAssignedAppsitesDto>();
        }
        [DataMember]
        public string CampignName { get; set; }
        [DataMember]
        public IList<CampaignAssignedAppsitesDto> CampaignAssignedAppsitesList { get; set; }
    }
    [DataContract]
    public class CampaignAssignedAppsitesDto : AssignedAppsitesDto
    {

    }
}
