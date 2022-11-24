using System.Collections.Generic;
using System.Linq;
using NHibernate.Linq;
using ArabyAds.AdFalcon.Domain.Common.Model.Campaign;
using ArabyAds.AdFalcon.Domain.Model.AppSite;
using ArabyAds.AdFalcon.Domain.Model.Campaign;
using ArabyAds.AdFalcon.Domain.Repositories.Campaign;
using ArabyAds.Framework.Persistence;

namespace ArabyAds.AdFalcon.Persistence.Repositories.Campaign
{
    public class AdSupportedCreativeUnitRepository : RepositoryBase<AdSupportedCreativeUnit, int>, IAdSupportedCreativeUnitRepository
    {
        public AdSupportedCreativeUnitRepository(RepositoryImplBase<AdSupportedCreativeUnit, int> repository)
            : base(repository)
        {
        }

        public IList<AdSupportedCreativeUnit> GetByAdType(AdTypeIds adType)
        {
            return UnitOfWork.Current.EntitySet<AdSupportedCreativeUnit>().WithOptions( op => op.SetCacheable(true)).Where(x => (x.AdType == null || x.AdType.ID == (int)adType)).ToList();
        }
    }
}
