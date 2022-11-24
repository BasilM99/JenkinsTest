using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArabyAds.AdFalcon.Domain.Model.Campaign.Targeting
{
    public class URLTargeting : TargetingBase
    {
        public virtual string URL { get; set; }
        public override string GetDescription()
        {
            return URL;
        }
        public override TargetingBase Copy()
        {
            var cloneObj = new URLTargeting()
            {
                URL = this.URL,
                AdGroup = this.AdGroup,
                Type = this.Type,
                IsDeleted = this.IsDeleted
            };
            return cloneObj;
        }
       
    }
}
