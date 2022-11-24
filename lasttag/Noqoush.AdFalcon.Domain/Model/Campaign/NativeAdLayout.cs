using System.Runtime.Serialization;
using Noqoush.AdFalcon.Domain.Model.AppSite;
using Noqoush.AdFalcon.Domain.Model.Core;
using Noqoush.AdFalcon.Domain.Repositories.Campaign;
using Noqoush.AdFalcon.Domain.Model.Campaign.Objective;
using System.Collections.Generic;

namespace Noqoush.AdFalcon.Domain.Model.Campaign
{
   
    public class NativeAdLayout : LookupBase<NativeAdLayout, int>
    {



        public virtual string Description { get; set; }
      
        public virtual string Code { get; set; }
    }

}
