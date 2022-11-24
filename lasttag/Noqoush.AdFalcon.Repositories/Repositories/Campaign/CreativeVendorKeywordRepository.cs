
using Noqoush.AdFalcon.Domain.Model.AppSite;
using Noqoush.AdFalcon.Domain.Model.Campaign;
using Noqoush.AdFalcon.Domain.Repositories.Campaign;
using Noqoush.Framework.Persistence;

namespace Noqoush.AdFalcon.Persistence.Repositories.Campaign
{
    public class CreativeVendorKeywordRepository : RepositoryBase<CreativeVendorKeyword, int>, ICreativeVendorKeywordRepository
    {
        public CreativeVendorKeywordRepository(RepositoryImplBase<CreativeVendorKeyword, int> repository)
            : base(repository)
        {
        }
    }
}
