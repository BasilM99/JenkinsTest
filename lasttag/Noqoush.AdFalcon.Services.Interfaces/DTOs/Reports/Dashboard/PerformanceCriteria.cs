using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Noqoush.AdFalcon.Domain.Common.Model.Campaign;

namespace Noqoush.AdFalcon.Services.Interfaces.DTOs.Reports.Dashboard
{
    [DataContract]
    public class PerformanceCriteriaBase
    {
        public PerformanceCriteriaBase()
        {
            Ids = new List<int>();
        }
        [DataMember]
        public IList<int> Ids { get; set; }

        public string GetIds(){return string.Join(",", Ids);}
        [DataMember]
        public CampaignType CampaignType { get; set; }
        [DataMember]
        public CampaignType OtherCampaignType { get; set; }
    }

    [DataContract]
    public class PerformanceCriteria : PerformanceCriteriaBase
    {
        [DataMember]
        public DateTime? FromDate { get; set; }
        [DataMember]
        public DateTime? ToDate { get; set; }
    }
}
