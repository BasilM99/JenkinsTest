using Noqoush.AdFalcon.Domain.Model.Campaign;
using Noqoush.AdFalcon.Domain.Model.Core;
using Noqoush.AdFalcon.Domain.Repositories.Campaign.Creative;
using Noqoush.Framework.Persistence;

namespace Noqoush.AdFalcon.Persistence.Repositories.Campaign.Creative
{
    public class AdCreativeUnitRepository : RepositoryBase<AdCreativeUnit, int>, IAdCreativeUnitRepository
    {
        public AdCreativeUnitRepository(RepositoryImplBase<AdCreativeUnit, int> repository)
            : base(repository)
        {
        }
    }
   
}
