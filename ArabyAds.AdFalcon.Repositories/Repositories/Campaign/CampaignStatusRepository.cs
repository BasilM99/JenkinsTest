using NHibernate.Linq;
using ArabyAds.AdFalcon.Domain.Model.AppSite;
using ArabyAds.AdFalcon.Domain.Model.Campaign;
using ArabyAds.AdFalcon.Domain.Repositories.Campaign;
using ArabyAds.Framework.Persistence;
using System.Collections.Generic;
using System.Linq;

namespace ArabyAds.AdFalcon.Persistence.Repositories.Campaign
{
    public class CampaignStatusRepository  : RepositoryBase<Domain.Model.Campaign.AdCampaignStatus, int>, ICampaignStatusRepository 
    {
        public CampaignStatusRepository(RepositoryImplBase<Domain.Model.Campaign.AdCampaignStatus, int> repository)
            : base(repository)
        {
        }

        public override IEnumerable<AdCampaignStatus> GetAll()
        {
            return UnitOfWork.Current.EntitySet<AdCampaignStatus>().WithOptions(op => op.SetCacheable(true)).ToList();
        }
    }
}
