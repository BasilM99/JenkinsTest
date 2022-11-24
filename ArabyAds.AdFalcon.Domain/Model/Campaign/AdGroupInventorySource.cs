using ArabyAds.AdFalcon.Domain.Model.Core;
using ArabyAds.Framework.DomainServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ArabyAds.AdFalcon.Domain.Model.Campaign
{



    public class AdGroupInventorySource : CampaignAssignedAppsite
    {
    
        public virtual AdGroup AdGroup { get; set; }

        public virtual ArabyAds.AdFalcon.Domain.Model.Core.SSPPartner Partner { get; set; }

       
 
        public override string GetDescription()
        {
            return this.Include.ToString();
        }

        public virtual AdGroupInventorySource CloneInventorySource()
        {
            var cloneObj = new AdGroupInventorySource()
            {
                Include = this.Include,
                IsDeleted = this.IsDeleted,
                AppSite = this.AppSite,
                Partner = this.Partner,
                Account = this.Account,
                SubAppsite = this.SubAppsite,
                SubPublisherId = this.SubPublisherId

            };
            return cloneObj;

        }


        //public virtual void Delete()
        //{
        //    this.IsDeleted = true;
        //}
    }
}
