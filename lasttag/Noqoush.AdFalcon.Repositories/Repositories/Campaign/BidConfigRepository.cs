using Noqoush.AdFalcon.Domain.Model.AppSite;
using Noqoush.AdFalcon.Domain.Repositories.Campaign;
using Noqoush.Framework.Persistence;

namespace Noqoush.AdFalcon.Persistence.Repositories.Campaign
{
    public class BidConfigRepository : RepositoryBase<Domain.Model.Campaign.BidConfig, int>, IBidConfigRepository
    {
        public BidConfigRepository(RepositoryImplBase<Domain.Model.Campaign.BidConfig, int> repository)
            : base(repository)
        {
        }
    }
}
