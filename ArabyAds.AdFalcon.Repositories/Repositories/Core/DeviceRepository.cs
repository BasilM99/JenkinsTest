using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArabyAds.Framework.Persistence;
using ArabyAds.AdFalcon.Domain.Model.Core;
using ArabyAds.AdFalcon.Domain.Repositories.Core;

namespace ArabyAds.AdFalcon.Persistence.Repositories.Core
{
    public class DeviceRepository : RepositoryBase<Device, int>, IDeviceRepository
    {
        public DeviceRepository(RepositoryImplBase<Device, int> repository)
            : base(repository)
        {

        }
    }

    public class DeviceCodeRepository : RepositoryBase<DeviceCode, int>, IDeviceCodeRepository
    {
        public DeviceCodeRepository(RepositoryImplBase<DeviceCode, int> repository)
            : base(repository)
        {

        }
    }

}
