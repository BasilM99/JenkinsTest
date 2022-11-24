using Noqoush.AdFalcon.Domain.Model.AppSite;
using Noqoush.AdFalcon.Domain.Model.Campaign;
using Noqoush.AdFalcon.Domain.Repositories.Campaign;
using Noqoush.Framework.Persistence;

namespace Noqoush.AdFalcon.Persistence.Repositories.Campaign
{
    public class AppMarketingPartnerRepository: RepositoryBase<AppMarketingPartner, int>, IAppMarketingPartnerRepository
    {
        public AppMarketingPartnerRepository(RepositoryImplBase<AppMarketingPartner, int> repository)
            : base(repository)
        {
        }
    }
}
