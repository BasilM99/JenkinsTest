using Noqoush.AdFalcon.Domain.Model.AppSite;
using Noqoush.AdFalcon.Domain.Model.Campaign;
using Noqoush.AdFalcon.Domain.Model.Campaign.Targeting;
using Noqoush.AdFalcon.Domain.Repositories.Campaign;
using Noqoush.Framework.Persistence;

namespace Noqoush.AdFalcon.Persistence.Repositories.Campaign
{
    public class AdvertiserAccountUserRepository : RepositoryBase<AdvertiserAccountUser, int>, IAdvertiserAccountUserRepository
    {
        public AdvertiserAccountUserRepository(RepositoryImplBase<AdvertiserAccountUser, int> repository)
            : base(repository)
        {
        }
    }
 
    
}
