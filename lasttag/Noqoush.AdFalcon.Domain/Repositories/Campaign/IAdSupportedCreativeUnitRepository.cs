using System.Collections.Generic;
using Noqoush.AdFalcon.Domain.Common.Model.Campaign;
using Noqoush.AdFalcon.Domain.Model.Campaign;
using Noqoush.Framework.Persistence;

namespace Noqoush.AdFalcon.Domain.Repositories.Campaign
{
    public interface IAdSupportedCreativeUnitRepository : IKeyedRepository<AdSupportedCreativeUnit, int>
    {
        IList<AdSupportedCreativeUnit> GetByAdType(AdTypeIds adType);

    }
}
