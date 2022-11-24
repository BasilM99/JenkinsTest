using ArabyAds.AdFalcon.Domain.Model.Account;
using ArabyAds.AdFalcon.Domain.Model.Core;
using ArabyAds.AdFalcon.Domain.Repositories;
using ArabyAds.AdFalcon.Domain.Repositories.Account;
using ArabyAds.AdFalcon.Domain.Repositories.Campaign;
using ArabyAds.AdFalcon.Domain.Repositories.Core;
using ArabyAds.Framework.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArabyAds.AdFalcon.Persistence.Repositories.Core
{
    public class DeviceLanguageRepository : RepositoryBase<DeviceLanguage, int>, IDeviceLanguageRepository
    {
        public DeviceLanguageRepository(RepositoryImplBase<DeviceLanguage, int> repository)
            : base(repository)
        {
        }

    }

    /*
    public class ViewAbilityVendorRepository : RepositoryBase<ViewAbilityVendor, int>, IViewAbilityVendorRepository
    {
        public ViewAbilityVendorRepository(RepositoryImplBase<ViewAbilityVendor, int> repository)
            : base(repository)
        {
        }

    }*/
}
