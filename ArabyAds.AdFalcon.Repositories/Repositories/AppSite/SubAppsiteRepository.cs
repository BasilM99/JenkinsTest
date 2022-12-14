using ArabyAds.AdFalcon.Domain.Model.Account.Discount;
using ArabyAds.AdFalcon.Domain.Repositories;
using ArabyAds.AdFalcon.Domain.Repositories.Core;
using ArabyAds.Framework.Persistence;
using ArabyAds.AdFalcon.Domain.Model.Account.SSP;
using ArabyAds.AdFalcon.Domain.Repositories.Account.SSP;
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
using ArabyAds.AdFalcon.Domain.Model.Core;
using System.Linq;
using ArabyAds.AdFalcon.Domain.Model.AppSite;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs;
namespace ArabyAds.AdFalcon.Repositories.Repositories
{
   

    public class SubAppsiteRepository : RepositoryBase<SubAppsite, int>, ISubAppsiteRepository
    {
        public SubAppsiteRepository(RepositoryImplBase<SubAppsite, int> repository)
            : base(repository)
        {

        }

        public IEnumerable<SubAppsiteTransfomer> GetSubAppSitesQuery(AllAppSiteCriteria criteria, out int Count)
        {
            ISession nhibernateSession = UnitOfWork.Current.OrmSession<NHibernate.ISession>();
            SubAppsite SubAppsiteAlias = null;
            AppSite AppSiteAlias = null;
            SubAppsiteTransfomer dto = null;

            IQueryOver<SubAppsite, SubAppsite> rootQuery = nhibernateSession.QueryOver<SubAppsite>(() => SubAppsiteAlias);


            //rootQuery.JoinAlias(M=>M.AppSite, ()=> AppSiteAlias);

            var AppSiteSubQuery = QueryOver.Of<AppSite>()
   .Where(gm => gm.ID == SubAppsiteAlias.AppSite.ID)
   
   .Select(gm => gm.Account.ID);

         

            //           var SSPPartnerSubQuery = QueryOver.Of<BusinessPartner>()
            //.Where(gm => gm.Account == AppSiteAlias.Account)
            //.Select(gm => gm.ID);

            //if (criteria.ExchangeIds != null && criteria.ExchangeIds.Length>0)
            //{
            //    SSPPartnerSubQuery.Where(M=>M.ID.IsIn(criteria.ExchangeIds));

            //}
            if (criteria.AccountIds != null && criteria.AccountIds.Length > 0)
            {
                var appsitesQ = nhibernateSession.QueryOver<AppSite>().Select(gm => gm.ID)
                    .Where(M => M.Account.ID.IsIn(criteria.AccountIds));
                var appsitesIds = appsitesQ.List<int>();
                rootQuery.Where(M => M.AppSite.ID.IsIn(appsitesIds.ToArray()));
            }


            if (!string.IsNullOrEmpty(criteria.QuickSearchField))
            {

                Disjunction disjun = new Disjunction();

                disjun.Add(Restrictions.Where(()=> SubAppsiteAlias.SubPublisherName.IsInsensitiveLike(criteria.QuickSearchField.ToLower(), MatchMode.Anywhere)));
                disjun.Add(Restrictions.Where(() => SubAppsiteAlias.SubPublisherId.IsInsensitiveLike(criteria.QuickSearchField.ToLower(), MatchMode.Anywhere)));
               // disjun.Add(Restrictions.Where(() => AppSiteAlias.Name.IsInsensitiveLike(criteria.QuickSearchField, MatchMode.Anywhere)));


                
                rootQuery.Where(disjun);
                //SSPPartnerSubQuery.Where(M => M.Name.IsInsensitiveLike(criteria.QuickSearchField, MatchMode.Anywhere));
                //AppSiteSubQuery.Where(M => M.Name.IsInsensitiveLike(criteria.QuickSearchField, MatchMode.Anywhere));
               // AppSiteAccSubQuery.Where(M => M.Name.IsInsensitiveLike(criteria.QuickSearchField, MatchMode.Anywhere));



            }
            //rootQuery.WithSubquery.WhereExists(SSPPartnerSubQuery);
            // rootQuery.WithSubquery.WhereExists(AppSiteSubQuery);


            var CountQuery = rootQuery.ToRowCountQuery();



            var projections = new List<IProjection>();
     
            projections.Add(Projections.Property(() => SubAppsiteAlias.SubPublisherId).WithAlias(() => dto.SubPublisherId));
            projections.Add(Projections.Property(() => SubAppsiteAlias.SubPublisherName).WithAlias(() => dto.SubPublisherName));
            projections.Add(Projections.Property(() => SubAppsiteAlias.ID).WithAlias(() => dto.Id));
            projections.Add(Projections.Property(() => SubAppsiteAlias.SubPublisherMarketId).WithAlias(() => dto.SubPublisherMarketId));
            projections.Add(Projections.Property(()=>SubAppsiteAlias.AppSite.ID).WithAlias(() => dto.AppSiteId));
            projections.Add(Projections.SubQuery(AppSiteSubQuery).WithAlias(() => dto.AccountId
            ));
            //projections.Add(Projections.SubQuery(AppSiteSubQuery).WithAlias(() => dto.AppSiteName));
           // projections.Add(Projections.SubQuery(SSPPartnerSubQuery).WithAlias(() => dto.ExchangeId));
            rootQuery.Select(projections.ToArray());
        



          
            rootQuery.TransformUsing(Transformers.AliasToBean<SubAppsiteTransfomer>());



            if (criteria.Page >= 0)
            {
                var pageIndex = criteria.Page;
                //rootQuery.OrderBy(item => item.FirstName);
                rootQuery.Skip(pageIndex * criteria.Size)
                                         .Take(criteria.Size);
                if (!string.IsNullOrEmpty(criteria.FieldName) && criteria.FieldName!="Id")
                {
                    if (criteria.Desc)
                        rootQuery.OrderBy(Projections.Property(criteria.FieldName)).Desc();
                    else
                        rootQuery.OrderBy(Projections.Property(criteria.FieldName)).Asc();
                }
                else
                {

                    rootQuery.OrderBy(M => M.ID).Desc();
                }
            }
            else
            {

                if (!string.IsNullOrEmpty(criteria.FieldName) && criteria.FieldName != "Id")
                {
                    if (criteria.Desc)
                        rootQuery.OrderBy(Projections.Property(criteria.FieldName)).Desc();
                    else
                        rootQuery.OrderBy(Projections.Property(criteria.FieldName)).Asc();
                }
                else
                {

                    rootQuery.OrderBy(M => M.ID).Desc();
                }
            }

            Count = CountQuery.SingleOrDefault<int>();
            return rootQuery.List<SubAppsiteTransfomer>();
        }
    }
}
