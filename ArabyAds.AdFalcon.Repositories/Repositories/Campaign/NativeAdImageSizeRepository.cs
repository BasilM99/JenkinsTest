using ArabyAds.AdFalcon.Domain.Model.AppSite;
using ArabyAds.AdFalcon.Domain.Model.Campaign;
using ArabyAds.AdFalcon.Domain.Repositories.Campaign;
using ArabyAds.Framework.Persistence;

namespace ArabyAds.AdFalcon.Persistence.Repositories.Campaign
{
    public class NativeAdImageSizeRepository : RepositoryBase<NativeAdImageSize, int>, INativeAdImageSizeRepository
    {
        public NativeAdImageSizeRepository(RepositoryImplBase<NativeAdImageSize, int> repository)
            : base(repository)
        {
        }
    }
}
