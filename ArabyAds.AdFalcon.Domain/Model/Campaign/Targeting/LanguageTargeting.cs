using ArabyAds.AdFalcon.Domain.Model.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArabyAds.AdFalcon.Domain.Model.Campaign.Targeting
{
  

    public class LanguageTargeting : TargetingBase
    {
        public virtual Language Language { get; set; }
        public override string GetDescription()
        {
            return Language.GetDescription();
        }
        public override TargetingBase Copy()
        {
            var cloneObj = new LanguageTargeting()
            {
                Language = this.Language,
                AdGroup = this.AdGroup,
                Type = this.Type,
                IsDeleted = this.IsDeleted
            };
            return cloneObj;
        }
    }
}
