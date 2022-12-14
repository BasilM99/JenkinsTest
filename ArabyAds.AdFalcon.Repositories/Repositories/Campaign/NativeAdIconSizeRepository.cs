using ArabyAds.AdFalcon.Domain.Model.AppSite;
using ArabyAds.AdFalcon.Domain.Model.Campaign;
using ArabyAds.AdFalcon.Domain.Repositories.Campaign;
using ArabyAds.Framework.Persistence;

namespace ArabyAds.AdFalcon.Persistence.Repositories.Campaign
{
    public class NativeAdIconSizeRepository : RepositoryBase<NativeAdIconSize, int>, INativeAdIconSizeRepository
    {
        public NativeAdIconSizeRepository(RepositoryImplBase<NativeAdIconSize, int> repository)
            : base(repository)
        {
        }
    }
}
