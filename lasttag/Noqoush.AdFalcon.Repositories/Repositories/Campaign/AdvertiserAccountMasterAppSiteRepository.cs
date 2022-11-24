
using Noqoush.AdFalcon.Domain.Model.AppSite;
using Noqoush.AdFalcon.Domain.Model.Campaign;
using Noqoush.AdFalcon.Domain.Repositories.Campaign;
using Noqoush.Framework.Persistence;

namespace Noqoush.AdFalcon.Persistence.Repositories.Campaign
{


    public class AdvertiserAccountMasterAppSiteRepository : RepositoryBase<AdvertiserAccountMasterAppSite, int>, IAdvertiserAccountMasterAppSiteRepository
    {

        public AdvertiserAccountMasterAppSiteRepository(RepositoryImplBase<AdvertiserAccountMasterAppSite, int> repository)
          : base(repository)
        {
        }
    }
}