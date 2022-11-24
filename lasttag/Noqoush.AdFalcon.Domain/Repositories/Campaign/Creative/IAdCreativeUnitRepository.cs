using Noqoush.AdFalcon.Domain.Model.Campaign;
using Noqoush.AdFalcon.Domain.Model.Core;
using Noqoush.Framework.Persistence;

namespace Noqoush.AdFalcon.Domain.Repositories.Campaign.Creative
{
    public interface IAdCreativeUnitRepository : IKeyedRepository<AdCreativeUnit, int>
    {

    }

    public interface IAdCreativeUnitAttributeMappingRepository : IKeyedRepository<AdCreativeUnitAttributeMapping, int>
    {

    }
}
