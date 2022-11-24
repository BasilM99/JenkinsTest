using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArabyAds.AdFalcon.Domain.Model.Campaign.Targeting
{
    public  class MasterAppSiteTargeting : TargetingBase   
    {

        public virtual AdvertiserAccountMasterAppSite List { get; set; }

        public override string GetDescription()
        {
            return string.Format("{0}", List.GetDescription());
        }
        public override TargetingBase Copy()
        {
            var cloneObj = new MasterAppSiteTargeting()
            {
                List = this.List,

                AdGroup = this.AdGroup,
                Type = this.Type,
                IsDeleted = this.IsDeleted
            };
            return cloneObj;
        }
    }
}
