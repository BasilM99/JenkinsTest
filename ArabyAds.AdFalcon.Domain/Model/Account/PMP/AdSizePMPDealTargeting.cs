using ArabyAds.AdFalcon.Domain.Model.Core;
using ArabyAds.AdFalcon.Domain.Repositories.Campaign.Creative;
using ArabyAds.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArabyAds.AdFalcon.Domain.Model.Account.PMP
{



    public class AdSizePMPDealTargeting : PMPDealTargeting
    {
        ICreativeUnitRepository _creativeUnitRepository = IoC.Instance.Resolve<ICreativeUnitRepository>();
        public virtual CreativeUnit AdSize
        {
            get;
            set;
        }
        public virtual string GetAdSizeDescription(string id)
        {
            return _creativeUnitRepository.Get(Convert.ToInt32(id)).ToString();
        }

    }
}
