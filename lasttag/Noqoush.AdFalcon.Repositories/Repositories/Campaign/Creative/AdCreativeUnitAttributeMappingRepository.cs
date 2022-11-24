using Noqoush.AdFalcon.Domain.Model.Campaign;
using Noqoush.AdFalcon.Domain.Model.Core;
using Noqoush.AdFalcon.Domain.Repositories.Campaign.Creative;
using Noqoush.Framework.Persistence;

namespace Noqoush.AdFalcon.Persistence.Repositories.Campaign.Creative
{
    public class AdCreativeUnitAttributeMappingRepository : RepositoryBase<AdCreativeUnitAttributeMapping, int>, IAdCreativeUnitAttributeMappingRepository
    {
        public AdCreativeUnitAttributeMappingRepository(RepositoryImplBase<AdCreativeUnitAttributeMapping, int> repository)
            : base(repository)
        {
        }
    }
}
