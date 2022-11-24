using ArabyAds.AdFalcon.Domain.Model.Campaign;
using ArabyAds.AdFalcon.Domain.Model.Core;
using ArabyAds.AdFalcon.Domain.Repositories.Campaign.Creative;
using ArabyAds.Framework.Persistence;

namespace ArabyAds.AdFalcon.Persistence.Repositories.Campaign.Creative
{
    public class AdCreativeUnitAttributeMappingRepository : RepositoryBase<AdCreativeUnitAttributeMapping, int>, IAdCreativeUnitAttributeMappingRepository
    {
        public AdCreativeUnitAttributeMappingRepository(RepositoryImplBase<AdCreativeUnitAttributeMapping, int> repository)
            : base(repository)
        {
        }
    }
}
