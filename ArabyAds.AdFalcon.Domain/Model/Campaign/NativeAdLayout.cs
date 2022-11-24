using System.Runtime.Serialization;
using ArabyAds.AdFalcon.Domain.Model.AppSite;
using ArabyAds.AdFalcon.Domain.Model.Core;
using ArabyAds.AdFalcon.Domain.Repositories.Campaign;
using ArabyAds.AdFalcon.Domain.Model.Campaign.Objective;
using System.Collections.Generic;

namespace ArabyAds.AdFalcon.Domain.Model.Campaign
{
   
    public class NativeAdLayout : LookupBase<NativeAdLayout, int>
    {



        public virtual string Description { get; set; }
      
        public virtual string Code { get; set; }
    }

}
