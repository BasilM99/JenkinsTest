using System;
using ArabyAds.AdFalcon.Domain.Model.Core;
using ArabyAds.AdFalcon.Domain.Model;
using ArabyAds.Framework.DomainServices;
using System.Collections.Generic;
using System.Linq;
using ArabyAds.AdFalcon.Domain.Model.AppSite;
namespace ArabyAds.AdFalcon.Domain.Model.Campaign
{
    public class AppMarketingPartner : ManagedLookupBase
    {

        public AppMarketingPartner()
        {

        }


        public virtual string Code { get; set; }

       
        public virtual string Description { get; protected set; }
     
        public virtual AppSite.AppSite AppSite { get; set; }
 

        public virtual IList<AppMarketingPartnerTracker> Trackers { get; set; }
    }
}
