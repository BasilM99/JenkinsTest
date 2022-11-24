using NHibernate;
using NHibernate.Criterion;
using NHibernate.Transform;
using Noqoush.AdFalcon.Domain.Repositories;
using Noqoush.Framework.DomainServices.AuditTrial;
using Noqoush.Framework.Persistence;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;
using NHibernate.Criterion.Lambda;
using Noqoush.AdFalcon.Domain.Repositories.Account.PMP;
using Noqoush.AdFalcon.Domain.Model.Account.PMP;
using Noqoush.AdFalcon.Domain.Model.Campaign;
using Noqoush.AdFalcon.Domain.Model.Campaign.Targeting;
using Noqoush.Framework;
using Noqoush.AdFalcon.Common.UserInfo;
using Noqoush.AdFalcon.Domain.Common.Model.Account.PMP;
using Noqoush.AdFalcon.Domain.Common.Model.Campaign;

namespace Noqoush.AdFalcon.Persistence.Repositories.Account.PMP
{
    public class PMPDealRepository : RepositoryBase<PMPDeal, int>, IPMPDealRepository
    {
        public PMPDealRepository(RepositoryImplBase<PMPDeal, int> repository)
            : base(repository)
        {
        }

        public IEnumerable<PMPDeal> GetPMPDeals(PMPDealCriteria filter, out int TotalCount)
        {


            PMPDeal PMPDealAlias = null;
            ISession nhibernateSession = UnitOfWork.Current.OrmSession<ISession>();

            IQueryOver<PMPDeal, PMPDeal> rootQuery = nhibernateSession.QueryOver<PMPDeal>(() => PMPDealAlias);
            var countQuery = nhibernateSession.QueryOver<PMPDeal>(() => PMPDealAlias);

            if (filter.ExchangeFiltred != null && (filter.ExchangeFiltred.Count > 0))
            {

                rootQuery.Where(M => M.Exchange.ID.IsIn(filter.ExchangeFiltred.ToArray()));
                countQuery.Where(M => M.Exchange.ID.IsIn(filter.ExchangeFiltred.ToArray()));

            }
           
            if (filter.AdSizes != null && (filter.AdSizes.Count > 0))
            {

                var AdSizePMPDealTargetings = QueryOver.Of<AdSizePMPDealTargeting>().Select(M => M.ID)
           .Where(M => M.Deal.ID == PMPDealAlias.ID).Where(M => M.IsDeleted == false).Where(M => M.AdSize.ID.IsIn(filter.AdSizes.ToArray()));



                rootQuery.WithSubquery.WhereExists(AdSizePMPDealTargetings);
                countQuery.WithSubquery.WhereExists(AdSizePMPDealTargetings);


            }
            if (filter.AdFormats != null && (filter.AdFormats.Count > 0))
            {

                var enumArray = filter.AdFormats.Select(x => (AdTypeGroup)x).ToArray();
                var AdTypeGroupPMPDealTargetings = QueryOver.Of<AdTypeGroupPMPDealTargeting>().Select(M => M.ID)
         .Where(M => M.Deal.ID == PMPDealAlias.ID).Where(M => M.IsDeleted == false).Where(M => M.AdTypeGroup.IsIn(enumArray)); ;



                rootQuery.WithSubquery.WhereExists(AdTypeGroupPMPDealTargetings);
                countQuery.WithSubquery.WhereExists(AdTypeGroupPMPDealTargetings);


            }

            if (filter.Geographies != null && (filter.Geographies.Count > 0))
            {


                var GeographicPMPDealTargetings = QueryOver.Of<GeographicPMPDealTargeting>().Select(M => M.ID)
                 .Where(M => M.Deal.ID == PMPDealAlias.ID).Where(M => M.IsDeleted == false).Where(M => M.Location.ID.IsIn(filter.Geographies.ToArray()));



                rootQuery.WithSubquery.WhereExists(GeographicPMPDealTargetings);
                countQuery.WithSubquery.WhereExists(GeographicPMPDealTargetings);


            }
            if (filter.OnlyGlobal)
            {
                rootQuery.Where(M => M.IsGlobal);
                countQuery.Where(M => M.IsGlobal);

                //if (filter.userId > 0)
                //{
                //    rootQuery.Where(M => M.User.ID == filter.userId);
                //    countQuery.Where(M => M.User.ID == filter.userId);

                //}

                //if (filter.AccountId > 0)
                //{
                //    rootQuery.Where(M => M.Account.ID == filter.AccountId);
                //    countQuery.Where(M => M.Account.ID == filter.AccountId);

                //}

            }
            if (!filter.OnlyGlobal && !filter.IsGlobal)
            {
                var disjunction = new Disjunction();
                var Conjuction = new Conjunction();
                var Conjuction2 = new Conjunction();
                if (filter.userId > 0)
                {
                    Conjuction.Add<PMPDeal>(M => M.User.ID == filter.userId);


                }
                if (filter.AccountId > 0)
                {
                    Conjuction.Add<PMPDeal>(M => M.Account.ID == filter.AccountId);


                }

                Conjuction.Add<PMPDeal>(M => M.IsGlobal == false);
                if (filter.AdvertiserAccountId.HasValue && filter.ShowAdvertiser)
                {
                   // Conjuction.Add<PMPDeal>(M => M.Advertiser.ID == filter.AdvertiserId);
                    Conjuction.Add<PMPDeal>(M => M.AdvertiserAccount.ID == filter.AdvertiserAccountId);

                }
                else if (filter.AdvertiserAccountId.HasValue)
                {
                    //Conjuction.Add<PMPDeal>(M => M.Advertiser == null);
                    Conjuction.Add<PMPDeal>(M => M.AdvertiserAccount == null);

                    if (filter.userId > 0)
                    {
                        Conjuction2.Add<PMPDeal>(M => M.User.ID == filter.userId);


                    }
                    if (filter.AccountId > 0)
                    {
                        Conjuction2.Add<PMPDeal>(M => M.Account.ID == filter.AccountId);


                    }
                    //Conjuction2.Add<PMPDeal>(M => M.Advertiser.ID == filter.AdvertiserId);
                    Conjuction2.Add<PMPDeal>(M => M.AdvertiserAccount.ID == filter.AdvertiserAccountId);
                    disjunction.Add(Conjuction2);
                }
                else
                {
                    //Conjuction.Add<PMPDeal>(M => M.Advertiser == null);
                    Conjuction.Add<PMPDeal>(M => M.AdvertiserAccount == null);
                }
                disjunction.Add(Conjuction);

                rootQuery.Where(disjunction);
                countQuery.Where(disjunction);
            }
            else if(!filter.OnlyGlobal)
            {
                var disjunction = new Disjunction();
                var Conjuction = new Conjunction();
                var Conjuction2 = new Conjunction();
                var Conjuction3 = new Conjunction();
                if (filter.userId > 0)
                {
                    Conjuction.Add<PMPDeal>(M => M.User.ID == filter.userId);


                }
                if (filter.AccountId > 0)
                {
                    Conjuction.Add<PMPDeal>(M => M.Account.ID == filter.AccountId);


                }
                //Conjuction.Add<PMPDeal>(M => M.Advertiser == null);
                Conjuction.Add<PMPDeal>(M => M.AdvertiserAccount == null);
                disjunction.Add(Conjuction);

                Conjuction2.Add<PMPDeal>(M => M.IsGlobal == true);
                //Conjuction2.Add<PMPDeal>(M => M.IsDeleted == false);
                //Conjuction2.Add<PMPDeal>(M => M.Advertiser == null);
                Conjuction2.Add<PMPDeal>(M => M.AdvertiserAccount == null);

                if (filter.AdvertiserAccountId.HasValue)
                {
                   // Conjuction.Add<PMPDeal>(M => M.Advertiser == null);

                    Conjuction.Add<PMPDeal>(M => M.AdvertiserAccount == null);
                    if (filter.userId > 0)
                    {
                        Conjuction3.Add<PMPDeal>(M => M.User.ID == filter.userId);


                    }
                    if (filter.AccountId > 0)
                    {
                        Conjuction3.Add<PMPDeal>(M => M.Account.ID == filter.AccountId);


                    }
                   // Conjuction3.Add<PMPDeal>(M => M.Advertiser.ID == filter.AdvertiserId);
                    Conjuction3.Add<PMPDeal>(M => M.AdvertiserAccount.ID == filter.AdvertiserAccountId);
                    disjunction.Add(Conjuction3);
                }
                disjunction.Add(Conjuction2);
                rootQuery.Where(disjunction);
                countQuery.Where(disjunction);

            }

            if (filter.Archived.HasValue)
            {
                if (filter.Archived == false)
                {
                    rootQuery.Where(M => M.IsDeleted == filter.Archived);

                    countQuery.Where(M => M.IsDeleted == filter.Archived);
                }
                

            }
            else
            {

                rootQuery.Where(M => M.IsDeleted == false);
                countQuery.Where(M => M.IsDeleted == false);
            }

            if (!string.IsNullOrEmpty(filter.PublisherName))
            {
                rootQuery.Where(M => M.PublisherName.IsInsensitiveLike(filter.PublisherName.ToLower(), MatchMode.Anywhere));
                countQuery.Where(M => M.PublisherName.IsInsensitiveLike(filter.PublisherName.ToLower(), MatchMode.Anywhere));

            }
            //if (filter.PublisherId > 0)
            //{
            //    rootQuery.Where(M => M.Publisher.ID == filter.PublisherId);
            //    countQuery.Where(M => M.Publisher.ID == filter.PublisherId);

            //}

            if (filter.ExchangeId > 0)
            {
                rootQuery.Where(M => M.Exchange.ID == filter.ExchangeId);
                countQuery.Where(M => M.Exchange.ID == filter.ExchangeId);

            }
            
            if (filter.Type != DealType.Undefined)
            {
                rootQuery.Where(M => M.Type == filter.Type);
                countQuery.Where(M => M.Type == filter.Type);

            }

            if (!string.IsNullOrEmpty(filter.Name))
            {
                rootQuery.Where(M => M.Name.IsInsensitiveLike(filter.Name.ToLower(), MatchMode.Anywhere));
                countQuery.Where(M => M.Name.IsInsensitiveLike(filter.Name.ToLower(), MatchMode.Anywhere));

            }
            if (filter.DateFrom.HasValue)
            {
                rootQuery.Where(M => M.StartDate >= filter.DateFrom);
                countQuery.Where(M => M.StartDate >= filter.DateFrom);

            }

            if (filter.DateTo.HasValue)
            {
                rootQuery.Where(M => M.StartDate <= filter.DateTo);
                countQuery.Where(M => M.StartDate <= filter.DateTo);

            }
           


            if (filter.Page > 0)
            {
                int pageIndexM = filter.Page.Value - 1;


                rootQuery.OrderBy(M => M.CreationDate).Desc();
                rootQuery.Skip(pageIndexM * filter.Size)
                                         .Take(filter.Size);

            }
            else
            {


                rootQuery.OrderBy(M => M.CreationDate).Desc();

            }
            //rootQuery.TransformUsing(Transformers.AliasToBean<AuditTrialDto>());
            TotalCount = countQuery.ToRowCountQuery().SingleOrDefault<int>();
            //.Select(
            // Projections.Distinct(



            //      Projections.ProjectionList()
            //         .Add(Projections.Property<PMPDeal>(x => x.ID))



            //     )



            //     )

            //.List<int>().Count
            //;
            return rootQuery.List<PMPDeal>();
        }


        public IList<PMPDeal> GetAllPMPDealsByAccount(int AccountId)
        {
            IList<PMPDeal> PMPDeals = Query(x => (!x.IsDeleted && x.Account != null && x.Account.ID == AccountId) || (x.Account != null && x.IsGlobal == true)).ToList();

            return PMPDeals;

        }

        public bool IsCampsBydeal(int DealId)
        {

            ISession nhibernateSession = UnitOfWork.Current.OrmSession<ISession>();

            AdGroup AdGroupAlias = null;
            var rootQuery = nhibernateSession.QueryOver<AdGroup>(() => AdGroupAlias);

            var AdPMPDealTargetingIdSubQuery = QueryOver.Of<AdPMPDealTargeting>().Select(M => M.ID)
            .Where(M => M.Deal.ID == DealId)
            .Where(M => M.AdGroup.ID == AdGroupAlias.ID)
            ;
            var Campaigns = rootQuery.WithSubquery.WhereExists(AdPMPDealTargetingIdSubQuery).Select(M => M.Campaign).List<Domain.Model.Campaign.Campaign>();
            IList<Domain.Model.Campaign.Campaign> camps = new List<Domain.Model.Campaign.Campaign>();
            if (Campaigns != null)
            {
                foreach (var Camp in Campaigns)
                {
                    if (camps.Where(M => M.ID == Camp.ID).FirstOrDefault() == null && (Camp.Account.ID == OperationContext.Current.UserInfo<AdFalconUserInfo>().AccountId.Value || (OperationContext.Current.CurrentPrincipal.IsInRole("Administrator") || OperationContext.Current.CurrentPrincipal.IsInRole("AdOps") || OperationContext.Current.CurrentPrincipal.IsInRole("AccountManager"))))
                        camps.Add(Camp);
                }
            }
            return camps.Count > 0;
        }

        public IList<Domain.Model.Campaign.AdvertiserAccount> getAdvertiserAccountsBydeal(int DealId)
        {

            ISession nhibernateSession = UnitOfWork.Current.OrmSession<ISession>();

            AdGroup AdGroupAlias = null;
            var rootQuery = nhibernateSession.QueryOver<AdGroup>(() => AdGroupAlias);

            var AdPMPDealTargetingIdSubQuery = QueryOver.Of<AdPMPDealTargeting>().Select(M => M.ID)
            .Where(M => M.Deal.ID == DealId)
            .Where(M => M.AdGroup.ID == AdGroupAlias.ID);
            var Campaigns = rootQuery.WithSubquery.WhereExists(AdPMPDealTargetingIdSubQuery).Select(M => M.Campaign.AdvertiserAccount).List<Domain.Model.Campaign.AdvertiserAccount>();
            IList<Domain.Model.Campaign.AdvertiserAccount> camps = new List<Domain.Model.Campaign.AdvertiserAccount>();
            if (Campaigns != null)
            {
                foreach (var Camp in Campaigns)
                {
                    if (camps.Where(M => M.ID == Camp.ID).FirstOrDefault() == null && (Camp.Account.ID == OperationContext.Current.UserInfo<AdFalconUserInfo>().AccountId.Value || (OperationContext.Current.CurrentPrincipal.IsInRole("Administrator") || OperationContext.Current.CurrentPrincipal.IsInRole("AdOps") || OperationContext.Current.CurrentPrincipal.IsInRole("AccountManager"))))
                        camps.Add(Camp);
                }
            }
            return camps;
        }
        public IList<Domain.Model.Campaign.Campaign> getCampsBydeal(int DealId)
        {

            ISession nhibernateSession = UnitOfWork.Current.OrmSession<ISession>();

            AdGroup AdGroupAlias = null;
            var rootQuery = nhibernateSession.QueryOver<AdGroup>(() => AdGroupAlias);

            var AdPMPDealTargetingIdSubQuery = QueryOver.Of<AdPMPDealTargeting>().Select(M => M.ID)
            .Where(M => M.Deal.ID == DealId)
            .Where(M => M.AdGroup.ID == AdGroupAlias.ID);
            var Campaigns = rootQuery.WithSubquery.WhereExists(AdPMPDealTargetingIdSubQuery).Select(M => M.Campaign).List<Domain.Model.Campaign.Campaign>();
            IList<Domain.Model.Campaign.Campaign> camps = new List<Domain.Model.Campaign.Campaign>();
            if (Campaigns != null)
            {
                foreach (var Camp in Campaigns)
                {
                    if (camps.Where(M => M.ID == Camp.ID).FirstOrDefault() == null && (Camp.Account.ID == OperationContext.Current.UserInfo<AdFalconUserInfo>().AccountId.Value || (OperationContext.Current.CurrentPrincipal.IsInRole("Administrator") || OperationContext.Current.CurrentPrincipal.IsInRole("AdOps") || OperationContext.Current.CurrentPrincipal.IsInRole("AccountManager"))))
                        camps.Add(Camp);
                }
            }
            return camps;
        }

        public IList<AdGroup> getDealCampsAdgruops(int dealId, int campId)
        {

            ISession nhibernateSession = UnitOfWork.Current.OrmSession<ISession>();

            AdGroup AdGroupAlias = null;
            var rootQuery = nhibernateSession.QueryOver<AdGroup>(() => AdGroupAlias);

            var AdPMPDealTargetingIdSubQuery = QueryOver.Of<AdPMPDealTargeting>().Select(M => M.ID)
            .Where(M => M.Deal.ID == dealId)
           .Where(M => M.AdGroup.ID == AdGroupAlias.ID)
           .Where(() => AdGroupAlias.Campaign.ID == campId);

            var AdGroups = rootQuery.WithSubquery.WhereExists(AdPMPDealTargetingIdSubQuery).List<AdGroup>();

            return AdGroups;
        }

    }
}
