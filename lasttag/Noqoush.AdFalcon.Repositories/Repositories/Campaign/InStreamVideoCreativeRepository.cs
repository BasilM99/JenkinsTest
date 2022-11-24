
using Noqoush.AdFalcon.Domain.Model.AppSite;
using Noqoush.AdFalcon.Domain.Model.Campaign;
using Noqoush.AdFalcon.Domain.Model.Campaign.Targeting;
using Noqoush.AdFalcon.Domain.Repositories.Campaign;
using Noqoush.Framework.Persistence;

namespace Noqoush.AdFalcon.Persistence.Repositories.Campaign
{
    public class InStreamVideoCreativeRepository : RepositoryBase<InStreamVideoCreative, int>, IInStreamVideoCreativeRepository
    {
        public InStreamVideoCreativeRepository(RepositoryImplBase<InStreamVideoCreative, int> repository)
            : base(repository)
        {
        }
    }
}
