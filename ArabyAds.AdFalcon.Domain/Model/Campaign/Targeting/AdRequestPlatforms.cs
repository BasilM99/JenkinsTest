﻿using ArabyAds.AdFalcon.Domain.Model.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArabyAds.AdFalcon.Domain.Model.Campaign.Targeting
{
    public class AdRequestPlatform : LookupBase<AdRequestPlatform, int>
    {
        public virtual string Code { get; set; }
    }
}
