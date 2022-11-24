using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArabyAds.AdFalcon.Domain.Model.Core
{
    public class CreativeUnitGroup : LookupBase<CreativeUnitGroup, int>
    {
        public virtual string Description { get; set; }
        public virtual string Code { get; set; }
        public virtual IList<CreativeUnit> CreativeUnits { get; set; }
    }
}
