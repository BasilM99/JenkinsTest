using Noqoush.AdFalcon.Domain.Model.Campaign;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Noqoush.Framework.DomainServices;
using Noqoush.AdFalcon.Domain.Common.Model.Campaign;

namespace Noqoush.AdFalcon.Domain.Model.Account.PMP
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
