using System.Runtime.Serialization;

namespace Noqoush.AdFalcon.Services.Interfaces.DTOs.AppSite
{
    [DataContract]
    public class AppSiteApprovalDto
    {
        [DataMember]
        public int AppSiteId { get; set; }
        [DataMember]
        public int StatusId { get; set; }
        [DataMember]
        public string Comments { get; set; }
        [DataMember]
        public string AccountLanguage { get; set; }
        [DataMember]
        public AppSiteTypeDto Type { get; set; }
        [DataMember]
        public string[] NewKeywords { get; set; }

        [DataMember]
        public string[] DeletedKeywords { get; set; }

    }
}