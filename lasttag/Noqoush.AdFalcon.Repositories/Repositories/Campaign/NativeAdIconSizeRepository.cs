using Noqoush.AdFalcon.Domain.Model.AppSite;
using Noqoush.AdFalcon.Domain.Model.Campaign;
using Noqoush.AdFalcon.Domain.Repositories.Campaign;
using Noqoush.Framework.Persistence;

namespace Noqoush.AdFalcon.Persistence.Repositories.Campaign
{
    public class NativeAdIconSizeRepository : RepositoryBase<NativeAdIconSize, int>, INativeAdIconSizeRepository
    {
        public NativeAdIconSizeRepository(RepositoryImplBase<NativeAdIconSize, int> repository)
            : base(repository)
        {
        }
    }
}
