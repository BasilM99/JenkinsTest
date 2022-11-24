using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Reports.Performance;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;

namespace ArabyAds.AdFalcon.Services.Interfaces.Messages
{
    [ProtoContract]
    public class GetPerformanceDetailsRequest
    {
        [ProtoMember(1)]
        public string AccountName { set; get; }
        [ProtoMember(2)]
        public string AppSiteName { set; get; }

        [ProtoMember(3)]
        public BaseAppSitePerformanceDetailsCriteria Criteria { set; get; }
    }
}
