using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Noqoush.Framework.Utilities;

namespace Noqoush.AdFalcon.Services.Interfaces.DTOs.Reports
{
 

    [DataContract]
    public class CampaignCardinalityEstimatorDto : BaseCampaignResultDto
    {

        [DataMember]
        public string ClickCacheKey { get; set; }
        [DataMember]
        public string ImpCacheKey { get; set; }

        [DataMember]
        public bool FromCache { get; set; }
        [DataMember]
        public int CampaignId { get; set; }

        [DataMember]
        public int AdGroupId { get; set; }
        [DataMember]
        public int Date { get; set; }
        [DataMember]
        public int? TimeId { get; set; }
        [DataMember]
        public new string DateRange { get; set; }

        [DataMember]
        public byte[] impressions_estimator { get; set; }



                    [DataMember]
        public long unique_impressions { get; set; }
        [DataMember]
        public long unique_clicks { get; set; }


        [DataMember]
        public byte[] clicks_estimator { get; set; }

    }
}
