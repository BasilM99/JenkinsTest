using Noqoush.AdFalcon.Domain.Model.AppSite;
using Noqoush.AdFalcon.Domain.Model.Campaign.Targeting.Device;
using Noqoush.AdFalcon.Domain.Repositories.Campaign;
using Noqoush.Framework.Persistence;

namespace Noqoush.AdFalcon.Persistence.Repositories.Campaign
{
    public class DeviceTargetingTypeRepository : RepositoryBase<DeviceTargetingType, int>, IDeviceTargetingTypeRepository
    {
        public DeviceTargetingTypeRepository(RepositoryImplBase<DeviceTargetingType, int> repository)
            : base(repository)
        {
        }
    }
}
