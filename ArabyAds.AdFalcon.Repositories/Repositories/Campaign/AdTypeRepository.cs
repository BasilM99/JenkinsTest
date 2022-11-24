using ArabyAds.AdFalcon.Domain.Model.AppSite;
using ArabyAds.AdFalcon.Domain.Model.Campaign;
using ArabyAds.AdFalcon.Domain.Repositories.Campaign;
using ArabyAds.Framework.Persistence;

namespace ArabyAds.AdFalcon.Persistence.Repositories.Campaign
{
    public class AdTypeRepository : RepositoryBase<AdType, int>, IAdTypeRepository
    {
        public AdTypeRepository(RepositoryImplBase<AdType, int> repository)
            : base(repository)
        {
        }
    }
}
