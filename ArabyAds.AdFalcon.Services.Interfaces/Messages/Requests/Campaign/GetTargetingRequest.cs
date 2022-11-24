using ArabyAds.AdFalcon.Domain.Common.Model.Campaign;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;

namespace ArabyAds.AdFalcon.Services.Interfaces.Messages.Requests.Campaign
{
    [ProtoContract]
    public class GetTargetingRequest : CampaignIdAdgroupIdMessage
    {
        public GetTargetingRequest()
        {
            CampaignType = CampaignType.Normal;
            CampaignOtherType = CampaignType.ProgrammaticGuaranteed;
        }
        [ProtoMember(1)]
        public CampaignType CampaignType { get; set; }

        [ProtoMember(2)]
        public CampaignType CampaignOtherType { get; set; }
    }
}
