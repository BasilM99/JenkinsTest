using ArabyAds.AdFalcon.Domain.Model.Core;
using ArabyAds.Framework.DomainServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArabyAds.AdFalcon.Domain.Model.Campaign
{
    public class MetricVendor : ManagedLookupBase
    {
        public virtual string Code { get; set; }

        public virtual string Description { get; set; }


    }
}
