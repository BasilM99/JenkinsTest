using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Noqoush.Framework.Utilities;

namespace Noqoush.AdFalcon.Services.Interfaces.DTOs.Reports
{
    


    [DataContract]
    public class MinHashEstimatorDto : BaseCampaignResultDto
    {
        [DataMember]
        public byte[] estimator { get; set; }
        [DataMember]
        public int counter_value_id { get; set; }

        [DataMember]
        public string counter_code { get; set; }
        [DataMember]
        public long unique_requests { get; set; }
        [DataMember]
        public int dateid { get; set; }
        [DataMember]
        public string CacheKey { get; set; }




    }
}
