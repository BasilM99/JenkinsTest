using ArabyAds.AdFalcon.Domain.Model.Campaign;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArabyAds.Framework.DomainServices;
using ArabyAds.AdFalcon.Domain.Common.Model.Campaign;

namespace ArabyAds.AdFalcon.Domain.Model.Account.PMP
{
   

    public class AdTypeGroupPMPDealTargeting : PMPDealTargeting
    {
        public virtual AdTypeGroup AdTypeGroup
        {
            get;
            set;
        }
        public virtual string GetAdTypeGroupDescription(string newValure)
        {
            return newValure;
        }

    }
}
