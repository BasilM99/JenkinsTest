
using Noqoush.AdFalcon.Domain.Model.AppSite;
using Noqoush.AdFalcon.Domain.Repositories.Campaign;
using Noqoush.Framework.Persistence;

namespace Noqoush.AdFalcon.Persistence.Repositories.Campaign
{
    public class CreativeVendorRepository : RepositoryBase<Domain.Model.Campaign.CreativeVendor, int>, ICreativeVendorRepository
    {   public CreativeVendorRepository(RepositoryImplBase<Domain.Model.Campaign.CreativeVendor, int> repository)
            : base(repository)
        {
    }

}
    
}
