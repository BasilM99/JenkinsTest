using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProtoBuf;
using ArabyAds.Framework.Utilities;

namespace ArabyAds.AdFalcon.Services.Interfaces.DTOs.Reports
{
 

    [ProtoContract]
    public class CampaignCardinalityEstimatorDto : BaseCampaignResultDto
    {

       [ProtoMember(1)]
        public string ClickCacheKey { get; set; }
       [ProtoMember(2)]
        public string ImpCacheKey { get; set; }

       [ProtoMember(3)]
        public bool FromCache { get; set; }
       [ProtoMember(4)]
        public int CampaignId { get; set; }

       [ProtoMember(5)]
        public int AdGroupId { get; set; }
       [ProtoMember(6)]
        public int Date { get; set; }
       [ProtoMember(7)]
        public int? TimeId { get; set; }
       [ProtoMember(8)]
        public new string DateRange { get; set; }

       [ProtoMember(9)]
        public byte[] impressions_estimator { get; set; }



                   [ProtoMember(10)]
        public long unique_impressions { get; set; }
       [ProtoMember(11)]
        public long unique_clicks { get; set; }


       [ProtoMember(12)]
        public byte[] clicks_estimator { get; set; }

    }
}
