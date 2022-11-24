using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArabyAds.AdFalcon.Domain.Model.Account.PMP;
namespace ArabyAds.AdFalcon.Domain.Model.Campaign.Targeting
{
 

    public class AdPMPDealTargeting : TargetingBase
    {
        public virtual  PMPDeal Deal { get; set; }

        public override string GetDescription()
        {
            return string.Format("{0}", Deal.GetDescription());
        }
        public override TargetingBase Copy()
        {
            var cloneObj = new AdPMPDealTargeting()
            {
                Deal = this.Deal,
             
                AdGroup = this.AdGroup,
                Type = this.Type,
                IsDeleted = this.IsDeleted
            };
            return cloneObj;
        }
    }

}
