using Noqoush.AdFalcon.Domain.Model.AppSite;
using Noqoush.AdFalcon.Domain.Repositories.Campaign;
using Noqoush.Framework.Persistence;
using Noqoush.AdFalcon.Domain.Model.Campaign.Targeting;

namespace Noqoush.AdFalcon.Persistence.Repositories.Campaign
{
    public class AdRequestTypeRepository : RepositoryBase<AdRequestType, int>, IAdRequestTypeRepository
    {
        public AdRequestTypeRepository(RepositoryImplBase<AdRequestType, int> repository)
            : base(repository)
        {
        }
    }
}
