using ArabyAds.AdFalcon.Domain.Model.Campaign;
using ArabyAds.AdFalcon.Domain.Model.Core;
using ArabyAds.AdFalcon.Domain.Repositories.Campaign;
using ArabyAds.AdFalcon.Domain.Repositories.Campaign.Creative;
using ArabyAds.Framework.Persistence;

namespace ArabyAds.AdFalcon.Persistence.Repositories.Campaign.Creative
{
    public class AdRepository : RepositoryBase<AdCreative, int>, IAdRepository
    {
        public AdRepository(RepositoryImplBase<AdCreative, int> repository)
            : base(repository)
        {
        }
    }
}
