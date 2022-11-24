using Noqoush.AdFalcon.Domain.Model.Core;
using Noqoush.Framework.DomainServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Noqoush.AdFalcon.Domain.Model.Campaign
{
    public class MetricVendor : ManagedLookupBase
    {
        public virtual string Code { get; set; }

        public virtual string Description { get; set; }


    }
}
