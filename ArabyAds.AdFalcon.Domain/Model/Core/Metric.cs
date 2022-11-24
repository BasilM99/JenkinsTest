using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArabyAds.AdFalcon.Domain.Model.Core
{
    public class Metric : LookupBase<Metric, int>
    {
        public virtual string Code { get; set; }

        public virtual string MetricTarget { get; set; }

        public virtual string Color { get; set; }
    }

}
