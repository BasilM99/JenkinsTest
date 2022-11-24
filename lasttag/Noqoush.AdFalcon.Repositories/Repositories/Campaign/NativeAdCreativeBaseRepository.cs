using Noqoush.AdFalcon.Domain.Model.AppSite;
using Noqoush.AdFalcon.Domain.Repositories.Campaign;
using Noqoush.Framework.Persistence;
using Noqoush.AdFalcon.Domain.Model.Campaign.Targeting;
using Noqoush.AdFalcon.Domain.Model.Campaign;

namespace Noqoush.AdFalcon.Persistence.Repositories.Campaign
{
   

    public class NativeAdCreativeBaseRepository : RepositoryBase<NativeAdCreativeBase, int>, INativeAdCreativeBaseRepository
    {
        public NativeAdCreativeBaseRepository(RepositoryImplBase<NativeAdCreativeBase, int> repository)
            : base(repository)
        {

        }
    }
}
