
using ArabyAds.AdFalcon.Domain.Model.AppSite;
using ArabyAds.AdFalcon.Domain.Repositories.Campaign;
using ArabyAds.Framework.Persistence;

namespace ArabyAds.AdFalcon.Persistence.Repositories.Campaign
{
    public class CreativeVendorRepository : RepositoryBase<Domain.Model.Campaign.CreativeVendor, int>, ICreativeVendorRepository
    {   public CreativeVendorRepository(RepositoryImplBase<Domain.Model.Campaign.CreativeVendor, int> repository)
            : base(repository)
        {
    }

}
    
}
