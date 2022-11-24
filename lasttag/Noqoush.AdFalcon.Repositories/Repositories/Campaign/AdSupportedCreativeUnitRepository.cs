using System.Collections.Generic;
using System.Linq;
using Noqoush.AdFalcon.Domain.Common.Model.Campaign;
using Noqoush.AdFalcon.Domain.Model.AppSite;
using Noqoush.AdFalcon.Domain.Model.Campaign;
using Noqoush.AdFalcon.Domain.Repositories.Campaign;
using Noqoush.Framework.Persistence;

namespace Noqoush.AdFalcon.Persistence.Repositories.Campaign
{
    public class AdSupportedCreativeUnitRepository : RepositoryBase<AdSupportedCreativeUnit, int>, IAdSupportedCreativeUnitRepository
    {
        public AdSupportedCreativeUnitRepository(RepositoryImplBase<AdSupportedCreativeUnit, int> repository)
            : base(repository)
        {
        }

        public IList<AdSupportedCreativeUnit> GetByAdType(AdTypeIds adType)
        {
            return this.GetAll().Where(x => (x.AdType == null || x.AdType.ID == (int)adType)).ToList();
        }
    }
}
