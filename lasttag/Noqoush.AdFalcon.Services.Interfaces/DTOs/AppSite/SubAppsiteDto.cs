using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Noqoush.AdFalcon.Services.Interfaces.DTOs.AppSite
{
    [DataContract]
    public class SubAppsiteDto
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string SubPublisherId { get; set; }
        [DataMember]
        public string SubPublisherName { get; set; }
        [DataMember]
        public string SubPublisherUrl { get; set; }
        [DataMember]
        public string SubPublisherMarketId { get; set; }
        [DataMember]
        public int AppSiteId { get; set; }
        [DataMember]
        public int ExchangeId { get; set; }
        [DataMember]
        public string AppSiteName { get; set; }
        [DataMember]
        public string ExchangeName { get; set; }

        [DataMember]
        public bool IsAllowed { get; set; }

    }
}
