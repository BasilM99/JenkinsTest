using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;

namespace ArabyAds.AdFalcon.Services.Interfaces.DTOs.Reports
{
    [ProtoContract]
    public class CampaignTroubleshootingCriteria 
    {
        [ProtoMember(1)]
        public int AdGroupId { get; set; }

        [ProtoMember(2)]
        public int DealId { get; set; }

        [ProtoMember(3)]
        public DateTime FromDate { get; set; }

        [ProtoMember(4)]
        public DateTime ToDate { get; set; }

        [ProtoMember(5)]
        public int AccountId { get; set; }
    }
}
