using ArabyAds.AdFalcon.Domain.Model.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArabyAds.AdFalcon.Domain.Model.Campaign
{
    public class AdCreativeAttribute : ManagedLookupBase
    {
        public virtual int Code { get; set; }

        public virtual string Description { get; set; }

        public virtual bool IsSupported { get; set; }
    }
}
