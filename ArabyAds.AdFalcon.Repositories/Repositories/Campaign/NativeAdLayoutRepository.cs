
using ArabyAds.AdFalcon.Domain.Model.AppSite;
using ArabyAds.AdFalcon.Domain.Model.Campaign;
using ArabyAds.AdFalcon.Domain.Repositories.Campaign;
using ArabyAds.Framework.Persistence;

namespace ArabyAds.AdFalcon.Persistence.Repositories.Campaign
{
    public class NativeAdLayoutRepository : RepositoryBase<NativeAdLayout, int>, INativeAdLayoutRepository
    {
        public NativeAdLayoutRepository(RepositoryImplBase<NativeAdLayout, int> repository)
            : base(repository)
        {
        }
    }
}
