using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Noqoush.AdFalcon.Domain.Model.AppSite;
using Noqoush.AdFalcon.Domain.Repositories;
using Noqoush.Framework.Persistence;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Criterion.Lambda;
using NHibernate.SqlCommand;
using NHibernate.Transform;
namespace Noqoush.AdFalcon.Repositories.Repositories
{
    public class AppSiteRepository : RepositoryBase<AppSite, int>, IAppSiteRepository
    {
        public AppSiteRepository(RepositoryImplBase<AppSite, int> repository)
            : base(repository)
        {
        }
        public string GetObjectName(int Id)
        {

            ISession nhibernateSession = UnitOfWork.Current.OrmSession<ISession>();

            IQueryOver<AppSite, AppSite> rootQuery = nhibernateSession.QueryOver<AppSite>();

            //joins


            rootQuery.Where(
                        p => p.ID == Id);


            return rootQuery.Select(M => M.Name).SingleOrDefault<string>();


        }
        public IEnumerable<SubAppsite> QueryByCratiriaForSubAppSite(AllAppSiteCriteria criteria)
        {

            ISession nhibernateSession = UnitOfWork.Current.OrmSession<ISession>();

            IQueryOver<SubAppsite, SubAppsite> rootQuery = nhibernateSession.QueryOver<SubAppsite>();
            Noqoush.AdFalcon.Domain.Model.AppSite.AppSite appSiteAlies = null;
            //joins


            rootQuery.Where(
                        p => p.AppSite.ID == criteria.AppSiteId);

            if (!string.IsNullOrEmpty(criteria.SubPublisherId))
            {
                rootQuery.Where(
                       p => p.SubPublisherId == criteria.SubPublisherId);
            }
            if (criteria.Page > -1)
            {
                var pageIndex = criteria.Page - 1;

                rootQuery.OrderBy(item => item.ID);
                rootQuery.Skip(pageIndex * criteria.Size)
                                         .Take(criteria.Size);
            }
            else
            {

                rootQuery.OrderBy(item => item.ID);
            }
            return rootQuery.List<SubAppsite>();


        }

        public int QueryByCratiriaForSubAppSiteCount(AllAppSiteCriteria criteria)
        {

            ISession nhibernateSession = UnitOfWork.Current.OrmSession<ISession>();

            IQueryOver<SubAppsite, SubAppsite> rootQuery = nhibernateSession.QueryOver<SubAppsite>();
            Noqoush.AdFalcon.Domain.Model.AppSite.AppSite appSiteAlies = null;
            //joins


            rootQuery.Where(
                        p => p.AppSite.ID == criteria.AppSiteId);

            if (!string.IsNullOrEmpty(criteria.SubPublisherId))
            {
                rootQuery.Where(
                       p => p.SubPublisherId == criteria.SubPublisherId);
            }


            rootQuery.OrderBy(item => item.ID);

            return rootQuery.RowCount();


        }

        public int getAccountId(int id)
        {
            ISession nhibernateSession = UnitOfWork.Current.OrmSession<ISession>();
            int accountId = nhibernateSession.QueryOver<AppSite>().Where(x => x.ID == id).Select(
            Projections.Property<AppSite>(x => x.Account.ID)).SingleOrDefault<int>();

            return accountId;

        }

        public AppSiteServerSetting getServerSetting(int id)
        {
            ISession nhibernateSession = UnitOfWork.Current.OrmSession<ISession>();
            AppSiteServerSetting ServerSetting = nhibernateSession.QueryOver<AppSiteServerSetting>().Where(x => x.AppSite.ID == id).SingleOrDefault<AppSiteServerSetting>();

            return ServerSetting;

        }
    }
}
