using ArabyAds.AdFalcon.Domain.Model.Campaign;
using ArabyAds.AdFalcon.Domain.Model.Core;
using ArabyAds.Framework.Persistence;

namespace ArabyAds.AdFalcon.Domain.Repositories.Campaign.Creative
{
    public interface IAdCreativeUnitRepository : IKeyedRepository<AdCreativeUnit, int>
    {

    }

    public interface IAdCreativeUnitAttributeMappingRepository : IKeyedRepository<AdCreativeUnitAttributeMapping, int>
    {

    }
}
