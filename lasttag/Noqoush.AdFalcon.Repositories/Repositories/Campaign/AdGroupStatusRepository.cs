using Noqoush.AdFalcon.Domain.Model.AppSite;
using Noqoush.AdFalcon.Domain.Repositories.Campaign;
using Noqoush.Framework.Persistence;

namespace Noqoush.AdFalcon.Persistence.Repositories.Campaign
{
    public class AdGroupStatusRepository : RepositoryBase<Domain.Model.Campaign.AdGroupStatus, int>, IAdGroupStatusRepository
    {
        public AdGroupStatusRepository(RepositoryImplBase<Domain.Model.Campaign.AdGroupStatus, int> repository)
            : base(repository)
        {
        }
    }
}
