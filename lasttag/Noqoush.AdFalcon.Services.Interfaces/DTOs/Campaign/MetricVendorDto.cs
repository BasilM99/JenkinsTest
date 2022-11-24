using System;
using System.Runtime.Serialization;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Reports;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Core;

namespace Noqoush.AdFalcon.Services.Interfaces.DTOs.Campaign
{
    [DataContract]
    public class MetricVendorDto : LookupDto
    {
        [DataMember]
        public string Code { get; set; }
        [DataMember]
        public string Description { get; set; }
    }
}
