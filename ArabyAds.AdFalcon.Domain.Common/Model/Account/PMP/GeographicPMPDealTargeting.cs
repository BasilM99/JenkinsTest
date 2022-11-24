using Noqoush.AdFalcon.Domain.Model.Core;
using Noqoush.AdFalcon.Domain.Repositories.Core;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Noqoush.AdFalcon.Domain.Model.Account.PMP
{


    public class GeographicPMPDealTargeting : PMPDealTargeting
    {
   
        public virtual LocationBase Location
        {
            get;
            set;
        }
        public virtual string GetLocationDescription(string id)
        {
            return _locationRepository.Get(Convert.ToInt32(id)).GetDescription();
        }

    }
}
