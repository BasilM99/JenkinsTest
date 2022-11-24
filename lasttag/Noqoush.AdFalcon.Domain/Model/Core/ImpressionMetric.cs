using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Noqoush.AdFalcon.Domain.Model.Campaign;

namespace Noqoush.AdFalcon.Domain.Model.Core
{

    public class ImpressionMetric : ManagedLookupBase
    {
        public virtual string Description { get; set; }
        public virtual string Code { get; set; }
        public virtual ICollection<MetricVendor> MetricVendors { get; set; }
    }

}
