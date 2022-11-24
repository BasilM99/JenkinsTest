using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArabyAds.Framework.Persistence;
using ArabyAds.AdFalcon.Domain.Model.Core;
using ArabyAds.AdFalcon.Domain.Repositories.Core;

namespace ArabyAds.AdFalcon.Persistence.Repositories.Core
{
    public class DeviceCapabilityRepository : RepositoryBase<DeviceCapability, int>, IDeviceCapabilityRepository
    {
        public DeviceCapabilityRepository(RepositoryImplBase<DeviceCapability, int> repository)
            : base(repository)
        {

        }
    }
}
