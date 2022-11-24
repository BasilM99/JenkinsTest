using NHibernate.Linq;
using ArabyAds.AdFalcon.Domain.Model.AppSite;
using ArabyAds.AdFalcon.Domain.Model.Campaign;
using ArabyAds.AdFalcon.Domain.Repositories.Campaign;
using ArabyAds.Framework.Persistence;
using System.Collections.Generic;
using System.Linq;

namespace ArabyAds.AdFalcon.Persistence.Repositories.Campaign
{
    public class AdGroupStatusRepository : RepositoryBase<Domain.Model.Campaign.AdGroupStatus, int>, IAdGroupStatusRepository
    {
        public AdGroupStatusRepository(RepositoryImplBase<Domain.Model.Campaign.AdGroupStatus, int> repository)
            : base(repository)
        { }
            public override IEnumerable<AdGroupStatus> GetAll()
        {
            return UnitOfWork.Current.EntitySet<AdGroupStatus>().WithOptions(op => op.SetCacheable(true)).ToList();
        }
    
    }
}
