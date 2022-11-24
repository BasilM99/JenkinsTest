using Noqoush.AdFalcon.Domain.Model.Campaign;
using Noqoush.AdFalcon.Domain.Model.Core;
using Noqoush.AdFalcon.Domain.Repositories.Campaign;
using Noqoush.AdFalcon.Domain.Repositories.Campaign.Creative;
using Noqoush.Framework.Persistence;

namespace Noqoush.AdFalcon.Persistence.Repositories.Campaign.Creative
{
    public class AdRepository : RepositoryBase<AdCreative, int>, IAdRepository
    {
        public AdRepository(RepositoryImplBase<AdCreative, int> repository)
            : base(repository)
        {
        }
    }
}
