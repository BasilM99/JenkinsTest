using Noqoush.AdFalcon.Domain.Model.AppSite;
using Noqoush.AdFalcon.Domain.Repositories.Campaign;
using Noqoush.Framework.Persistence;

namespace Noqoush.AdFalcon.Persistence.Repositories.Campaign
{
    public class AdCreativeStatusRepository : RepositoryBase<Domain.Model.Campaign.AdCreativeStatus, int>, IAdCreativeStatusRepository 
    {
        public AdCreativeStatusRepository(RepositoryImplBase<Domain.Model.Campaign.AdCreativeStatus, int> repository)
            : base(repository)
        {
        }
    }
}
