using System.Collections.Generic;
using System.Runtime.Serialization;
using System;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Reports;
using Noqoush.Framework.Utilities;

namespace Noqoush.AdFalcon.Services.Interfaces.DTOs.Campaign
{
    [DataContract]
    public class AdvertiserAccountListResultDto
    {
        [DataMember]
        public AdvertiserPerformanceDto Performance { get; set; }
        [DataMember]
        public IEnumerable<AdvertiserAccountListDto> Items { get; set; }
        [DataMember]
        public long TotalCount { get; set; }
    }
    [DataContract]
    public class AdvertiserAccountListDto
    {
        [DataMember]
        public bool IsDeleted { get; set; }
        [DataMember]
        public string Status { get; set; }
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public AdvertiserPerformanceDto Performance { get; set; }

        [DataMember]
        public AdvertiserDto AdvertiserItem { get; set; }

        [DataMember]
        public int? AdvertiserId { get; set; }


        [DataMember]
        public string AdvertiserAccId { get { return Id + "-" + this.AdvertiserItem.ID; } set { } }

        public string IsDeletedString { get { return IsDeleted.ToString(); } }
    }
}
