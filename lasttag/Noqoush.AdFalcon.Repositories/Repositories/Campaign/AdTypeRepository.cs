using Noqoush.AdFalcon.Domain.Model.AppSite;
using Noqoush.AdFalcon.Domain.Model.Campaign;
using Noqoush.AdFalcon.Domain.Repositories.Campaign;
using Noqoush.Framework.Persistence;

namespace Noqoush.AdFalcon.Persistence.Repositories.Campaign
{
    public class AdTypeRepository : RepositoryBase<AdType, int>, IAdTypeRepository
    {
        public AdTypeRepository(RepositoryImplBase<AdType, int> repository)
            : base(repository)
        {
        }
    }
}
