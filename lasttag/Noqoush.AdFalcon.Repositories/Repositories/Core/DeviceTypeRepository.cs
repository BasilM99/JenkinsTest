using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Noqoush.Framework.Persistence;
using Noqoush.AdFalcon.Domain.Model.Core;
using Noqoush.AdFalcon.Domain.Repositories.Core;

namespace Noqoush.AdFalcon.Persistence.Repositories.Core
{
    public class DeviceTypeRepository : RepositoryBase<DeviceType, int>, IDeviceTypeRepository
    {
        public DeviceTypeRepository(RepositoryImplBase<DeviceType, int> repository)
            : base(repository)
        {

        }
    }
}
