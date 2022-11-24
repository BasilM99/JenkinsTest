using Noqoush.AdFalcon.Domain.Model.Core;
using Noqoush.AdFalcon.Domain.Repositories.Campaign.Creative;
using Noqoush.Framework.Persistence;

namespace Noqoush.AdFalcon.Persistence.Repositories.Campaign.Creative
{
    public class CreativeUnitRepository : RepositoryBase<CreativeUnit, int>, ICreativeUnitRepository
    {
        public CreativeUnitRepository(RepositoryImplBase<CreativeUnit, int> repository)
            : base(repository)
        {
        }
    }
}
