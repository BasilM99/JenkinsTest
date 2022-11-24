using System;
using Noqoush.AdFalcon.Domain.Model.Core;
using Noqoush.AdFalcon.Domain.Model;

using System.Collections.Generic;
using System.Linq;
using Noqoush.AdFalcon.Domain.Model.AppSite;
namespace Noqoush.AdFalcon.Domain.Model.Campaign
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
