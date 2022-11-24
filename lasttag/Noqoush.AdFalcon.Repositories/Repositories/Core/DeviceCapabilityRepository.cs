using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Noqoush.Framework.Persistence;
using Noqoush.AdFalcon.Domain.Model.Core;
using Noqoush.AdFalcon.Domain.Repositories.Core;

namespace Noqoush.AdFalcon.Persistence.Repositories.Core
{
    public class DeviceCapabilityRepository : RepositoryBase<DeviceCapability, int>, IDeviceCapabilityRepository
    {
        public DeviceCapabilityRepository(RepositoryImplBase<DeviceCapability, int> repository)
            : base(repository)
        {

        }
    }
}
