﻿using System;
using System.Collections.Generic;
using System.Linq;
using ProtoBuf;
using System.Text;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Core;

namespace ArabyAds.AdFalcon.Services.Interfaces.DTOs.Campaign.Targeting
{
    [ProtoContract]
    public class OperatorTargetingDto : TargetingBaseDto
    {
       [ProtoMember(1)]
        public OperatorDto Operator { get; set; }
    }
}
