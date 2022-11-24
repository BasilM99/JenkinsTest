using System.Collections.Generic;
using ArabyAds.AdFalcon.Domain.Common.Model.Campaign;
using ArabyAds.AdFalcon.Domain.Model.Campaign;
using ArabyAds.Framework.Persistence;

namespace ArabyAds.AdFalcon.Domain.Repositories.Campaign
{
    public interface IAdSupportedCreativeUnitRepository : IKeyedRepository<AdSupportedCreativeUnit, int>
    {
        IList<AdSupportedCreativeUnit> GetByAdType(AdTypeIds adType);

    }
}
