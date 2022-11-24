using ProtoBuf;

namespace ArabyAds.AdFalcon.Services.Interfaces.DTOs.AppSite
{
    [ProtoContract]
    public class AppSiteApprovalDto
    {
       [ProtoMember(1)]
        public int AppSiteId { get; set; }
       [ProtoMember(2)]
        public int StatusId { get; set; }
       [ProtoMember(3)]
        public string Comments { get; set; }
       [ProtoMember(4)]
        public string AccountLanguage { get; set; }
       [ProtoMember(5)]
        public AppSiteTypeDto Type { get; set; }
       [ProtoMember(6)]
        public string[] NewKeywords { get; set; }

       [ProtoMember(7)]
        public string[] DeletedKeywords { get; set; }
        [ProtoMember(8)]
        public int approveStatus { get; set; }
        [ProtoMember(9)]
        public int[] intKeywords { get; set; }
        



    }
}