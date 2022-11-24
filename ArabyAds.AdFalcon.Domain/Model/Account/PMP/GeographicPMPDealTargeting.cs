using ArabyAds.AdFalcon.Domain.Model.Core;
using ArabyAds.AdFalcon.Domain.Repositories.Core;
using ArabyAds.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArabyAds.AdFalcon.Domain.Model.Account.PMP
{


    public class GeographicPMPDealTargeting : PMPDealTargeting
    {
        ILocationRepository _locationRepository = IoC.Instance.Resolve<ILocationRepository>();
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
