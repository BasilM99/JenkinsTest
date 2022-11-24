using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArabyAds.AdFalcon.Domain.Model.Core
{
    public class metriceGroupColumn
    {
        public virtual metriceColumn metriceColumn { set; get; }
        public virtual metriceGroup metriceGroup { set; get; }
        public virtual string Deatils { set; get; }
    }
}
