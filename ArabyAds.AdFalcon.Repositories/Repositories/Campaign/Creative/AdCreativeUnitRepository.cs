using ArabyAds.AdFalcon.Domain.Model.Campaign;
using ArabyAds.AdFalcon.Domain.Model.Core;
using ArabyAds.AdFalcon.Domain.Repositories.Campaign.Creative;
using ArabyAds.Framework.Persistence;

namespace ArabyAds.AdFalcon.Persistence.Repositories.Campaign.Creative
{
    public class AdCreativeUnitRepository : RepositoryBase<AdCreativeUnit, int>, IAdCreativeUnitRepository
    {
        public AdCreativeUnitRepository(RepositoryImplBase<AdCreativeUnit, int> repository)
            : base(repository)
        {
        }
    }
   
}
