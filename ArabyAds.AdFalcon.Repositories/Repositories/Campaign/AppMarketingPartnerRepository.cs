using ArabyAds.AdFalcon.Domain.Model.AppSite;
using ArabyAds.AdFalcon.Domain.Model.Campaign;
using ArabyAds.AdFalcon.Domain.Repositories.Campaign;
using ArabyAds.Framework.Persistence;

namespace ArabyAds.AdFalcon.Persistence.Repositories.Campaign
{
    public class AppMarketingPartnerRepository: RepositoryBase<AppMarketingPartner, int>, IAppMarketingPartnerRepository
    {
        public AppMarketingPartnerRepository(RepositoryImplBase<AppMarketingPartner, int> repository)
            : base(repository)
        {
        }
    }
}
