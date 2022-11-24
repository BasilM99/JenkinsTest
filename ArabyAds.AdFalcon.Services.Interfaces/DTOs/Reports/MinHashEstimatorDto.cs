using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProtoBuf;
using ArabyAds.Framework.Utilities;

namespace ArabyAds.AdFalcon.Services.Interfaces.DTOs.Reports
{
    


    [ProtoContract]
    public class MinHashEstimatorDto : BaseCampaignResultDto
    {
       [ProtoMember(1)]
        public byte[] estimator { get; set; }
       [ProtoMember(2)]
        public int counter_value_id { get; set; }

       [ProtoMember(3)]
        public string counter_code { get; set; }
       [ProtoMember(4)]
        public long unique_requests { get; set; }
       [ProtoMember(5)]
        public int dateid { get; set; }
       [ProtoMember(6)]
        public string CacheKey { get; set; }




    }
}
