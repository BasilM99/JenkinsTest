using ArabyAds.AdFalcon.Domain.Common.Model.Core.CostElement;
using ArabyAds.Framework.DomainServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace ArabyAds.AdFalcon.Domain.Model.Core.CostElement
{

    public class CostElement: CostItem //LookupBase<CostElement, int>
    {
      
        public virtual bool IsOneTime { get; set; }
        public virtual int Scope { get; set; }
        public virtual long CalculatedFromFeeCategory { get; set; }
        public virtual CostElementCalculatedFrom CostElementCalculatedFrom { get; set; }
        public virtual string GetCustomDescription()
        {
            if (this.CostElementCalculatedFrom != CostElementCalculatedFrom.Undefined)
            {
                return this.Name.ToString() + " (" + this.CostElementCalculatedFrom.ToText() + ")"; ;
            }
            else
            {
                return this.Name.ToString();


            }
        }
    }
}
