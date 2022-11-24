using ArabyAds.AdFalcon.Domain.Model.AppSite;
using ArabyAds.AdFalcon.Domain.Repositories.Campaign;
using ArabyAds.Framework.Persistence;
using ArabyAds.AdFalcon.Domain.Model.Campaign.Targeting;
using ArabyAds.AdFalcon.Domain.Model.Campaign;

namespace ArabyAds.AdFalcon.Persistence.Repositories.Campaign
{
   

    public class NativeAdCreativeBaseRepository : RepositoryBase<NativeAdCreativeBase, int>, INativeAdCreativeBaseRepository
    {
        public NativeAdCreativeBaseRepository(RepositoryImplBase<NativeAdCreativeBase, int> repository)
            : base(repository)
        {

        }
    }
}
