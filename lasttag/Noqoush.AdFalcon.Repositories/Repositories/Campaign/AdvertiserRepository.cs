using Noqoush.AdFalcon.Domain.Model.AppSite;
using Noqoush.AdFalcon.Domain.Model.Campaign;
using Noqoush.AdFalcon.Domain.Repositories.Campaign;
using Noqoush.Framework.Persistence;

namespace Noqoush.AdFalcon.Persistence.Repositories.Campaign
{
    public class AdvertiserRepository : RepositoryBase<Advertiser, int>, IAdvertiserRepository
    {
        public AdvertiserRepository(RepositoryImplBase<Advertiser, int> repository)
            : base(repository)
        {
        }
    }
}
