using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;

namespace ArabyAds.AdFalcon.Services.Interfaces.Messages
{
    [ProtoContract]
    public class GetDealCampsAdgroupsRequest
    {
        [ProtoMember(1)]
        public int DealId { get; set; }
        [ProtoMember(2)]
        public int CampaignId { get; set; }

        public override string ToString()
        {
            return $"{DealId}_{CampaignId}";
        }
    }
}
