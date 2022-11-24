using System;
using System.Runtime.Serialization;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Reports;

namespace Noqoush.AdFalcon.Services.Interfaces.DTOs.Campaign
{
    [DataContract]
    public class SitesListDto
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public int BusinessId { get; set; }
        [DataMember]
        public string Name { get; set; }

    }
}
