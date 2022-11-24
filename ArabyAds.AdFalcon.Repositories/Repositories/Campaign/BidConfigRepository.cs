using ArabyAds.AdFalcon.Domain.Model.AppSite;
using ArabyAds.AdFalcon.Domain.Repositories.Campaign;
using ArabyAds.Framework.Persistence;

namespace ArabyAds.AdFalcon.Persistence.Repositories.Campaign
{
    public class BidConfigRepository : RepositoryBase<Domain.Model.Campaign.BidConfig, int>, IBidConfigRepository
    {
        public BidConfigRepository(RepositoryImplBase<Domain.Model.Campaign.BidConfig, int> repository)
            : base(repository)
        {
        }
    }
}
