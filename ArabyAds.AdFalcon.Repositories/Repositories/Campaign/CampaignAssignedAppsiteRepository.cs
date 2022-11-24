using ArabyAds.AdFalcon.Domain.Model.AppSite;
using ArabyAds.AdFalcon.Domain.Repositories.Campaign;
using ArabyAds.Framework.Persistence;
using System.Collections.Generic;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Criterion.Lambda;
using NHibernate.SqlCommand;
using NHibernate.Transform;
using ArabyAds.AdFalcon.Domain.Model.Account;
using ArabyAds.Framework;
using ArabyAds.AdFalcon.Common.UserInfo;
using NHibernate.Linq;
namespace ArabyAds.AdFalcon.Persistence.Repositories.Campaign
{
    public class CampaignAssignedAppsiteRepository : RepositoryBase<Domain.Model.Campaign.CampaignAssignedAppsite, int>, ICampaignAssignedAppsiteRepository
    {
        public CampaignAssignedAppsiteRepository(RepositoryImplBase<Domain.Model.Campaign.CampaignAssignedAppsite, int> repository)
            : base(repository)
        {
        }

        public IList<int> GetCampaignIdsByAppSiteId(int AppSiteId)
        {
            ISession nhibernateSession = UnitOfWork.Current.OrmSession<NHibernate.ISession>();
            Domain.Model.Campaign.CampaignAssignedAppsite CampaignAssignedAppsitelias = null;


            IQueryOver<Domain.Model.Campaign.CampaignAssignedAppsite, Domain.Model.Campaign.CampaignAssignedAppsite> rootQuery = nhibernateSession.QueryOver<Domain.Model.Campaign.CampaignAssignedAppsite>(() => CampaignAssignedAppsitelias);
            rootQuery.Select(Projections.Distinct(Projections.Property<Domain.Model.Campaign.CampaignAssignedAppsite>(x => x.Campaign.ID)));
            rootQuery.Where(M => M.AppSite.ID == AppSiteId);
            rootQuery.Where(M => M.IsDeleted==false);
           // rootQuery.Where(M => M.ad == false);
            return rootQuery.List<int>();
        }

    }
}
