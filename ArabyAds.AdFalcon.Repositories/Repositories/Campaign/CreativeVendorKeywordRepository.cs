
using ArabyAds.AdFalcon.Domain.Model.AppSite;
using ArabyAds.AdFalcon.Domain.Model.Campaign;
using ArabyAds.AdFalcon.Domain.Repositories.Campaign;
using ArabyAds.Framework.Persistence;

namespace ArabyAds.AdFalcon.Persistence.Repositories.Campaign
{
    public class CreativeVendorKeywordRepository : RepositoryBase<CreativeVendorKeyword, int>, ICreativeVendorKeywordRepository
    {
        public CreativeVendorKeywordRepository(RepositoryImplBase<CreativeVendorKeyword, int> repository)
            : base(repository)
        {
        }
    }
}
