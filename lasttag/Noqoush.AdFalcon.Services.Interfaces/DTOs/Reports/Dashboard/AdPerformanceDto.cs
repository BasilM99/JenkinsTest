using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Noqoush.Framework.Utilities;

namespace Noqoush.AdFalcon.Services.Interfaces.DTOs.Reports
{
    [DataContract]
    public class AdPerformanceDto : BaseCampaignResultDto
    {
        [DataMember]
        public Int64 TotalCount;

        [DataMember]
        public string CampaignName { get; set; }

    }
}
