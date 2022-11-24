using Noqoush.AdFalcon.Domain.Model.Account.Discount;
using Noqoush.AdFalcon.Domain.Repositories;
using Noqoush.AdFalcon.Domain.Repositories.Core;
using Noqoush.Framework.Persistence;
using Noqoush.AdFalcon.Domain.Model.Account.SSP;
using Noqoush.AdFalcon.Domain.Repositories.Account.SSP;
using System.Collections.Generic;
using Noqoush.AdFalcon.Domain.Model.AppSite;
using Noqoush.AdFalcon.Domain.Model.Core;
using Noqoush.AdFalcon.Domain.Repositories;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Criterion.Lambda;
using NHibernate.SqlCommand;
using NHibernate.Transform;

namespace Noqoush.AdFalcon.Persistence.Repositories.Core
{
    public class SiteZoneMappingRepository : RepositoryBase<SiteZoneMapping, int>, ISiteZoneMappingRepository
    {
        public SiteZoneMappingRepository(RepositoryImplBase<SiteZoneMapping, int> repository)
            : base(repository)
        {
        }

     

        public IEnumerable<SiteZoneMapping> QueryByCratiriaForSiteZoneMapping(SiteZoneMappingCriteria criteria)
        { 
            
            ISession nhibernateSession = UnitOfWork.Current.OrmSession<ISession>();

        IQueryOver<SiteZoneMapping, SiteZoneMapping> rootQuery = nhibernateSession.QueryOver<SiteZoneMapping>();
        Noqoush.AdFalcon.Domain.Model.AppSite.AppSite appSiteAlies = null;
            //joins
         

            rootQuery.Where(criteria.GetExpression());
            if (!string.IsNullOrEmpty(criteria.AppSiteName))
            {
                rootQuery.JoinAlias(M => M.AppSite, () => appSiteAlies);
                //rootQuery.Where(() => appSiteAlies.Name == criteria.AppSiteName);
                // to change it to get alies dynamc
                rootQuery.Where(Expression.Sql("UPPER(appsiteali1_.Name) like UPPER(?)", "%" + criteria.AppSiteName + "%", NHibernateUtil.String));
            }
          
            if (criteria.Page.HasValue)
            {
                var pageIndex = criteria.Page.Value - 1;
                rootQuery.OrderBy(item => item.ID);
                rootQuery.Skip(pageIndex * criteria.Size)
                                         .Take(criteria.Size);
            }
            else
            {

                rootQuery.OrderBy(item => item.ID);
            }
            return rootQuery.List<SiteZoneMapping>();


        }
    }
}
