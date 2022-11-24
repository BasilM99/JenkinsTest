using ArabyAds.AdFalcon.Domain.Model.Account.Discount;
using ArabyAds.AdFalcon.Domain.Repositories;
using ArabyAds.AdFalcon.Domain.Repositories.Core;
using ArabyAds.Framework.Persistence;
using ArabyAds.AdFalcon.Domain.Model.Account.SSP;
using ArabyAds.AdFalcon.Domain.Repositories.Account.SSP;

namespace ArabyAds.AdFalcon.Persistence.Repositories.Core
{
    public class FloorPriceRepository : RepositoryBase<FloorPrice, int>, IFloorPriceRepository
    {
        public FloorPriceRepository(RepositoryImplBase<FloorPrice, int> repository)
            : base(repository)
        {
        }
    }
}
