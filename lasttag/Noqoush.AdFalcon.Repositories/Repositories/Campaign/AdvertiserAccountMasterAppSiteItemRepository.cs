using Noqoush.AdFalcon.Domain.Model.AppSite;
using Noqoush.AdFalcon.Domain.Model.Campaign;
using Noqoush.AdFalcon.Domain.Repositories.Campaign;
using Noqoush.Framework.Persistence;

namespace Noqoush.AdFalcon.Persistence.Repositories.Campaign
{
    

    public class AdvertiserAccountMasterAppSiteItemRepository : RepositoryBase<AdvertiserAccountMasterAppSiteItem, int>, IAdvertiserAccountMasterAppSiteItemRepository
    {

        public AdvertiserAccountMasterAppSiteItemRepository(RepositoryImplBase<AdvertiserAccountMasterAppSiteItem, int> repository)
          : base(repository)
        {
        }
    }
}