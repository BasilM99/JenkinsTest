using ArabyAds.AdFalcon.Domain.Model.Campaign;
using ArabyAds.AdFalcon.Domain.Repositories.Campaign;
using ArabyAds.Framework.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate;
namespace ArabyAds.AdFalcon.Persistence.Repositories.Campaign
{
    class AdGroupRepository : RepositoryBase<AdGroup, int>, IAdGroupRepository
    {
        public AdGroupRepository(RepositoryImplBase<AdGroup, int> repository)
            : base(repository)
        {
        }

        public bool AdGroupHasAds(int id)
        {
            ISession nhibernateSession = UnitOfWork.Current.OrmSession<ISession>();
            IQueryOver<AdCreative, AdCreative> rootQuery = nhibernateSession.QueryOver<AdCreative>();
            return rootQuery.Where( p => !p.IsDeleted && p.Group.ID == id).Select(p => p.ID).Take(1).SingleOrDefault<int>()> 0;
        }

        public string GetObjectName(int Id)
        {

            ISession nhibernateSession = UnitOfWork.Current.OrmSession<ISession>();

            IQueryOver<AdGroup, AdGroup> rootQuery = nhibernateSession.QueryOver<AdGroup>();

            //joins


            rootQuery.Where(
                        p => p.ID == Id);


            return rootQuery.Select(M => M.Name).SingleOrDefault<string>();


        }

        public IList<AdGroup> GetAdGroupsByCampaign(int id)
        {
            return Query(x => (!x.IsDeleted && x.Campaign.ID == id)).ToList();
        }

    }
}
