
using Noqoush.AdFalcon.Domain.Model.AppSite;
using Noqoush.AdFalcon.Domain.Model.Campaign;
using Noqoush.AdFalcon.Domain.Repositories.Campaign;
using Noqoush.Framework.Persistence;

namespace Noqoush.AdFalcon.Persistence.Repositories.Campaign
{
    public class NativeAdLayoutRepository : RepositoryBase<NativeAdLayout, int>, INativeAdLayoutRepository
    {
        public NativeAdLayoutRepository(RepositoryImplBase<NativeAdLayout, int> repository)
            : base(repository)
        {
        }
    }
}
