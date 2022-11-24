using ArabyAds.AdFalcon.Domain.Model.Core;
using ArabyAds.AdFalcon.Domain.Repositories.Campaign.Creative;
using ArabyAds.Framework.Persistence;

namespace ArabyAds.AdFalcon.Persistence.Repositories.Campaign.Creative
{
    public class CreativeUnitRepository : RepositoryBase<CreativeUnit, int>, ICreativeUnitRepository
    {
        public CreativeUnitRepository(RepositoryImplBase<CreativeUnit, int> repository)
            : base(repository)
        {
        }
    }
}
