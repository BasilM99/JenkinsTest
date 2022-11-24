using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArabyAds.AdFalcon.Domain.Model.QueryBuilder
{
    public class Measure : TreeQB
    {
        public virtual bool SupportedByPublisher { set; get; }
        public virtual bool SupportedByAdvertiser { set; get; }

        public virtual int minWidth { set; get; }
    }
}
