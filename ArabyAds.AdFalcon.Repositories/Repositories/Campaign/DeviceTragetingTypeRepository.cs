using ArabyAds.AdFalcon.Domain.Model.AppSite;
using ArabyAds.AdFalcon.Domain.Model.Campaign.Targeting.Device;
using ArabyAds.AdFalcon.Domain.Repositories.Campaign;
using ArabyAds.Framework.Persistence;

namespace ArabyAds.AdFalcon.Persistence.Repositories.Campaign
{
    public class DeviceTargetingTypeRepository : RepositoryBase<DeviceTargetingType, int>, IDeviceTargetingTypeRepository
    {
        public DeviceTargetingTypeRepository(RepositoryImplBase<DeviceTargetingType, int> repository)
            : base(repository)
        {
        }
    }
}
