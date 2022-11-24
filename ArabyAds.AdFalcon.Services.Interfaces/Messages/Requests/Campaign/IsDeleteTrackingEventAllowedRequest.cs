﻿using ArabyAds.AdFalcon.Domain.Common.Model.Campaign;
using ArabyAds.AdFalcon.Domain.Common.Model.Core;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.AppSite;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Core;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace ArabyAds.AdFalcon.Services.Interfaces.Messages
{

    [ProtoContract]
    public class IsDeleteTrackingEventAllowedRequest : CampaignIdAdgroupIdMessage
    {
        [ProtoMember(1)]
        public List<string> AdGroupTrackingEventCodes { get; set; }

        [ProtoMember(2)]
        public bool CheckStandards { get; set; }

        [ProtoMember(3)]
        public int? NewCostModelWrapperId { get; set; }

        public override string ToString()
        {
            return $"{base.ToString()}_{AdGroupTrackingEventCodes?.ToString() ?? "Null"}_{CheckStandards}_{NewCostModelWrapperId?.ToString() ?? "Null"}";
        }
    }
   
}
