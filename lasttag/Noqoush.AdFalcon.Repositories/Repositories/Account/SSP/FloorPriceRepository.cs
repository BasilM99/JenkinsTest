using Noqoush.AdFalcon.Domain.Model.Account.Discount;
using Noqoush.AdFalcon.Domain.Repositories;
using Noqoush.AdFalcon.Domain.Repositories.Core;
using Noqoush.Framework.Persistence;
using Noqoush.AdFalcon.Domain.Model.Account.SSP;
using Noqoush.AdFalcon.Domain.Repositories.Account.SSP;

namespace Noqoush.AdFalcon.Persistence.Repositories.Core
{
    public class FloorPriceRepository : RepositoryBase<FloorPrice, int>, IFloorPriceRepository
    {
        public FloorPriceRepository(RepositoryImplBase<FloorPrice, int> repository)
            : base(repository)
        {
        }
    }
}
