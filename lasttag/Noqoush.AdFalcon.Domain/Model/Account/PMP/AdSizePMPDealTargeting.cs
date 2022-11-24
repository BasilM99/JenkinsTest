using Noqoush.AdFalcon.Domain.Model.Core;
using Noqoush.AdFalcon.Domain.Repositories.Campaign.Creative;
using Noqoush.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Noqoush.AdFalcon.Domain.Model.Account.PMP
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
