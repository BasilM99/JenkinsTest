using Noqoush.AdFalcon.Domain.Model.AppSite;
using Noqoush.AdFalcon.Domain.Model.Campaign;
using Noqoush.AdFalcon.Domain.Repositories.Campaign;
using Noqoush.Framework.Persistence;


namespace Noqoush.AdFalcon.Persistence.Repositories.Campaign
{
  

    public class AdGroupEventRepository : RepositoryBase<Domain.Model.Campaign.AdGroupEvent, int>, IAdGroupEventRepository
    {
        public AdGroupEventRepository(RepositoryImplBase<AdGroupEvent, int> repository)
            : base(repository)
        {
        }
    }
}
