using Noqoush.AdFalcon.Domain.Model.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Noqoush.AdFalcon.Domain.Model.Campaign.Targeting
{
  

    public class AdPositionTargeting : TargetingBase
    {
        public override string GetDescription()
        {
            return string.Empty;
        }


        public virtual IList<AdPosition> AdPositions { get; set; }

      
        public virtual bool PagePositionEnabled { get; set; }

        public override TargetingBase Copy()
        {
            var cloneObj = new AdPositionTargeting()
            {

                AdGroup = this.AdGroup,
                Type = this.Type,
                IsDeleted = this.IsDeleted,

                AdPositions = this.AdPositions,
                PagePositionEnabled = this.PagePositionEnabled,
             
            };



            return cloneObj;
        }

    }
}
