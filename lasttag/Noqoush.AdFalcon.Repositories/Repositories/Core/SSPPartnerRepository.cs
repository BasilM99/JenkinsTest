using Noqoush.AdFalcon.Domain.Model.Core;
using Noqoush.AdFalcon.Domain.Repositories.Campaign;
using Noqoush.Framework.Persistence;
using Noqoush.AdFalcon.Domain.Repositories.Core;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Criterion.Lambda;
using NHibernate.SqlCommand;
using NHibernate.Transform;
using NHibernate.Linq;
using System.Collections.Generic;

namespace Noqoush.AdFalcon.Persistence.Repositories.Core
{
 
    public class SSPPartnerRepository : RepositoryBase<SSPPartner, int>, ISSPPartnerRepository
    {
        public SSPPartnerRepository(RepositoryImplBase<SSPPartner, int> repository)
            : base(repository)
        {


        }
        public int CheckWeatherMeetGefoenceResricions(List<int> AppSiteIds)
        {

            ISession nhibernateSession = UnitOfWork.Current.OrmSession<ISession>();

            IQueryOver<SSPPartner, SSPPartner> rootQuery = nhibernateSession.QueryOver<SSPPartner>();

            //joins


            rootQuery.Where(
                        p => p.AppSite.ID.IsIn(AppSiteIds.ToArray()));

            rootQuery.Where(M => M.DisallowGeofenceLessThanRadius == true);
            var largestRadious = 0;
            var results = rootQuery.List<SSPPartner>();
            if (results == null || results.Count == 0)
            {
                return -1;
            }
            else
            {

                foreach (var partner in results)
                {

                    if (partner.GeofenceRadius> largestRadious)
                    {
                        largestRadious = partner.GeofenceRadius;

                    }
                }
            }
            return largestRadious;
        }
        public bool CheckWeatherMeetRTBSetting(int AppSiteId)
        {

            ISession nhibernateSession = UnitOfWork.Current.OrmSession<ISession>();

            IQueryOver<SSPPartner, SSPPartner> rootQuery = nhibernateSession.QueryOver<SSPPartner>();

            //joins


            rootQuery.Where(
                        p => p.AppSite.ID == AppSiteId);

            Disjunction dis = new Disjunction();

            Restrictions.Where<SSPPartner>(M => M.TaggingAllowed == false);
            Restrictions.Where<SSPPartner>(M => M.DisallowGeofenceLessThanRadius == false);
            rootQuery.Where(dis);
            var results= rootQuery.List<SSPPartner>();
            if (results==null || results.Count==0)
            {
                return false;
            }
            return true;
        }
        public List<int> CheckWeatherMeetRTBSettings()
        {

            ISession nhibernateSession = UnitOfWork.Current.OrmSession<ISession>();

            IQueryOver<SSPPartner, SSPPartner> rootQuery = nhibernateSession.QueryOver<SSPPartner>();

            //joins

            rootQuery.Select(M => M.AppSite.ID);

            Conjunction dis = new Conjunction();

            Restrictions.Where<SSPPartner>(M => M.TaggingAllowed == true);
            Restrictions.Where<SSPPartner>(M => M.DisallowGeofenceLessThanRadius == true);
            rootQuery.Where(dis);
            var results = rootQuery.List<int>();

            return (List<int>)results;
        }
        public List<int> CheckWeatherNotMeetRTBSettings()
        {

            ISession nhibernateSession = UnitOfWork.Current.OrmSession<ISession>();

            IQueryOver<SSPPartner, SSPPartner> rootQuery = nhibernateSession.QueryOver<SSPPartner>();

            //joins

            rootQuery.Select(M=>M.AppSite.ID);

                   Disjunction dis = new Disjunction();

            Restrictions.Where<SSPPartner>(M => M.TaggingAllowed == false);
            Restrictions.Where<SSPPartner>(M => M.DisallowGeofenceLessThanRadius == false);
            rootQuery.Where(dis);
            var results = rootQuery.List<int>();
           
            return (List<int>)results;
        }


        public List<int> CheckWeatherNotMeetRTBSettingsSSPPartner()
        {

            ISession nhibernateSession = UnitOfWork.Current.OrmSession<ISession>();

            IQueryOver<SSPPartner, SSPPartner> rootQuery = nhibernateSession.QueryOver<SSPPartner>();

            //joins

            rootQuery.Select(M => M.ID);

            Disjunction dis = new Disjunction();

            Restrictions.Where<SSPPartner>(M => M.TaggingAllowed == false);
            Restrictions.Where<SSPPartner>(M => M.DisallowGeofenceLessThanRadius == false);
            rootQuery.Where(dis);
            var results = rootQuery.List<int>();

            return (List<int>)results;
        }
    }
}
