
using Noqoush.AdFalcon.Domain.Model.Account.Discount;
using Noqoush.AdFalcon.Domain.Repositories;
using Noqoush.AdFalcon.Domain.Repositories.Core;
using Noqoush.Framework.Persistence;
using Noqoush.AdFalcon.Domain.Model.Account.SSP;
using Noqoush.AdFalcon.Domain.Repositories.Account.SSP;
using System.Collections.Generic;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Criterion.Lambda;
using NHibernate.SqlCommand;
using NHibernate.Transform;
namespace Noqoush.AdFalcon.Persistence.Repositories.Core
{
    public class DealCampaignMappingRepository : RepositoryBase<DealCampaignMapping, int>, IDealCampaignMappingRepository
    {
        public DealCampaignMappingRepository(RepositoryImplBase<DealCampaignMapping, int> repository)
            : base(repository)
        {
        }




        public IEnumerable<DealCampaignMapping> QueryByCratiriaForDealCampaignMapping(DealCampaignMappingCriteria criteria)
        { 
            
            ISession nhibernateSession = UnitOfWork.Current.OrmSession<ISession>();

            IQueryOver<DealCampaignMapping, DealCampaignMapping> rootQuery = nhibernateSession.QueryOver<DealCampaignMapping>();
            Noqoush.AdFalcon.Domain.Model.Campaign.Campaign campaignAlies = null;
            //joins
         

            rootQuery.Where(criteria.GetExpression());
            if (!string.IsNullOrEmpty(criteria.CampaignName))
            {
                rootQuery.JoinAlias(M => M.Campaign, () => campaignAlies);
                //rootQuery.Where(() => appSiteAlies.Name == criteria.AppSiteName);
                // to change it to get alies dynamc
                rootQuery.Where(Expression.Sql("UPPER(campaignal1_.Name) like UPPER(?)", "%" + criteria.CampaignName + "%", NHibernateUtil.String));
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
            return rootQuery.List<DealCampaignMapping>();


        }
    }
}
