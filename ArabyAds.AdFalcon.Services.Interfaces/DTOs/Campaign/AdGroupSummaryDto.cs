using System.Collections.Generic;
using System.Runtime.Serialization;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.AppSite;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Core;
using Noqoush.Framework.DataAnnotations;

namespace Noqoush.AdFalcon.Services.Interfaces.DTOs.Campaign
{
    [DataContract]
    public class AdGroupSummaryDto
    {
        [DataMember]
        public virtual int ID { get; set; }
        [DataMember]
        public int CampaignId { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string ActionType { get; set; }
        [DataMember]
        public string Objective { get; set; }
        [DataMember]
        public decimal Bid { get; set; }
        [DataMember]
        public string Status { get; set; }
    }
}
