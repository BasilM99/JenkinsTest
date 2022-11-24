using ArabyAds.AdFalcon.Domain.Model.AppSite;
using ArabyAds.AdFalcon.Domain.Repositories.Campaign;
using ArabyAds.Framework.Persistence;

namespace ArabyAds.AdFalcon.Persistence.Repositories.Campaign
{
    public class AdCreativeStatusRepository : RepositoryBase<Domain.Model.Campaign.AdCreativeStatus, int>, IAdCreativeStatusRepository 
    {
        public AdCreativeStatusRepository(RepositoryImplBase<Domain.Model.Campaign.AdCreativeStatus, int> repository)
            : base(repository)
        {
        }
    }
}
