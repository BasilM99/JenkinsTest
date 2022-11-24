using Noqoush.AdFalcon.Domain.Model.AppSite;
using Noqoush.AdFalcon.Domain.Model.Campaign;
using Noqoush.AdFalcon.Domain.Repositories.Campaign;
using Noqoush.Framework.Persistence;

namespace Noqoush.AdFalcon.Persistence.Repositories.Campaign
{
    public class NativeAdImageSizeRepository : RepositoryBase<NativeAdImageSize, int>, INativeAdImageSizeRepository
    {
        public NativeAdImageSizeRepository(RepositoryImplBase<NativeAdImageSize, int> repository)
            : base(repository)
        {
        }
    }
}
