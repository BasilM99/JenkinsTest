using Noqoush.AdFalcon.Domain.Model.Campaign;
using Noqoush.AdFalcon.Domain.Model.Core;
using Noqoush.AdFalcon.Domain.Repositories.Campaign.Creative;
using Noqoush.Framework.Persistence;

namespace Noqoush.AdFalcon.Persistence.Repositories.Campaign.Creative
{
    public class InStreamVideoCreativeUnitRepository : RepositoryBase<InStreamVideoCreativeUnit, int>, IInStreamVideoCreativeUnitRepository
    {
        public InStreamVideoCreativeUnitRepository(RepositoryImplBase<InStreamVideoCreativeUnit, int> repository)
            : base(repository)
        {
        }
    }
}
