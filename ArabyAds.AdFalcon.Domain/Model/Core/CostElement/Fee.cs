﻿using ArabyAds.AdFalcon.Domain.Common.Model.Core.CostElement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArabyAds.AdFalcon.Domain.Model.Core.CostElement
{
   public class Fee: CostItem
    {
        public virtual bool IsAutoAdded { get; set; }
        public virtual bool IsBillable { get; set; }
        public virtual FeeCalculatedFrom FeeCalculatedFrom { get; set; }
        
    }
}
