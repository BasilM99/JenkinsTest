using Noqoush.AdFalcon.Domain.Model.AppSite;
using Noqoush.AdFalcon.Domain.Repositories.Campaign;
using Noqoush.Framework.Persistence;

namespace Noqoush.AdFalcon.Persistence.Repositories.Campaign
{
    public class HouseAdRepository  : RepositoryBase<Domain.Model.Campaign.HouseAd, int>, IHouseAdRepository 
    {
        public HouseAdRepository(RepositoryImplBase<Domain.Model.Campaign.HouseAd, int> repository)
            : base(repository)
        {
        }
    }
}
