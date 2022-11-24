
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
using System;
using ArabyAds.AdFalcon.Domain.Common.Model.Account;

namespace ArabyAds.AdFalcon.Persistence.Repositories
{
    public class UserRepository : RepositoryBase<User, int>, IUserRepository
    {
        public UserRepository(RepositoryImplBase<User, int> repository)
            : base(repository)
        {
        }
        public IEnumerable<User> GetSSPPartners(AllAppSiteCriteria criteria, out int Count)
        {
            ISession nhibernateSession = UnitOfWork.Current.OrmSession<ISession>();
            User userAlias = null;
            ArabyAds.AdFalcon.Domain.Model.Account.Account AccountAlias = null;

  
            IQueryOver<User, User> rootQuery = nhibernateSession.QueryOver<User>(() => userAlias);

            ArabyAds.AdFalcon.Domain.Model.Account.UserAccounts UserAccountAlias = null;
            rootQuery.JoinAlias(() => userAlias.UserAccounts, () => UserAccountAlias)
                     .JoinAlias(() => UserAccountAlias.Account, () => AccountAlias);

         //   rootQuery.JoinAlias(() => userAlias.Account, () => AccountAlias);
            var AppSitesList = QueryOver.Of<ArabyAds.AdFalcon.Domain.Model.AppSite.AppSite>().Select(M => M.Account.ID)
           .Where(M => M.Account.ID == AccountAlias.ID);
            AppSitesList.Where(m => m.IsDeleted == false);

            var PrimaryUsers = QueryOver.Of<ArabyAds.AdFalcon.Domain.Model.Account.Account>().Select(M => M.PrimaryUser.ID)
       .Where(M => M.PrimaryUser.ID == userAlias.ID);

            var SSPPartners = QueryOver.Of<SSPPartner>().Select(M => M.ID)
       .Where(M => M.Account.ID == AccountAlias.ID);




            if (!string.IsNullOrEmpty(criteria.SubPublisherId))

            {
                ArabyAds.AdFalcon.Domain.Model.AppSite.SubAppsite SubAppsiteAlias = null;



                AppSitesList.JoinAlias(M => M.SubAppsites, () => SubAppsiteAlias);
                AppSitesList.Where(() => SubAppsiteAlias.SubPublisherId == criteria.SubPublisherId);


            }
            if (criteria.AccountId.HasValue)

            {
                rootQuery.Where(() => UserAccountAlias.Account.ID == criteria.AccountId);
            }
            if (criteria.DateFrom.HasValue)

            {
                AppSitesList.Where(M => M.RegistrationDate >= criteria.DateFrom);
            }
            if (criteria.UserId.HasValue)

            {
                rootQuery.Where(M => M.ID == criteria.UserId);
            }
            if (criteria.DateTo.HasValue)

            {
                AppSitesList.Where(M => M.RegistrationDate <= criteria.DateTo);
            }
            if (criteria.TypeId.HasValue)

            {
                AppSitesList.Where(M => M.Type.ID == criteria.TypeId);
            }
            if (criteria.StatusId.HasValue)

            {
                AppSitesList.Where(M => M.Status.ID == criteria.StatusId);
            }
            if (!string.IsNullOrEmpty(criteria.Name))

            {
                AppSitesList.Where(M => M.Name.IsInsensitiveLike(criteria.Name.ToLower(), MatchMode.Anywhere));

            }

            if (!string.IsNullOrEmpty(criteria.Email) || !string.IsNullOrEmpty(criteria.AccountName) || !string.IsNullOrEmpty(criteria.CompanyName))

            {

                ArabyAds.AdFalcon.Domain.Model.Account.Account AccountAppSIteAlias = null;
                ArabyAds.AdFalcon.Domain.Model.Account.User UserAccountAppSIteAlias = null;
                //AppSitesList.JoinAlias(M => M.Account, () => AccountAppSIteAlias);
                //AppSitesList.JoinAlias(() => AccountAppSIteAlias.PrimaryUser, () => UserAccountAppSIteAlias);
                if (!string.IsNullOrEmpty(criteria.Email))
                    SSPPartners.Where(M => M.Email == criteria.Email);
                if (!string.IsNullOrEmpty(criteria.AccountName))
                    SSPPartners.Where(M => M.Name.IsInsensitiveLike(criteria.AccountName.ToLower(), MatchMode.Anywhere));
                // rootQuery.Where(Expression.Sql("CONCAT(UPPER(this_.FirstName), ' ', UPPER(this_.LastName)) like UPPER(?)", "%" + criteria.AccountName + "%", NHibernateUtil.String));
                //if (!string.IsNullOrEmpty(criteria.AccountName))
                //    rootQuery.Where(() => userAlias.LastName.IsInsensitiveLike(criteria.AccountName.ToLower(), MatchMode.Anywhere));
                if (!string.IsNullOrEmpty(criteria.CompanyName))
                    rootQuery.Where(() => userAlias.Company.IsInsensitiveLike(criteria.CompanyName.ToLower(), MatchMode.Anywhere));
            }
            rootQuery.WithSubquery.WhereExists(AppSitesList);
            rootQuery.WithSubquery.WhereExists(PrimaryUsers);
            rootQuery.WithSubquery.WhereExists(SSPPartners);

            var CountQuery = rootQuery.ToRowCountQuery();

            if (criteria.Page >= 0)
            {
                var pageIndex = criteria.Page - 1;
                //rootQuery.OrderBy(item => item.FirstName);
                rootQuery.Skip(pageIndex * criteria.Size)
                                         .Take(criteria.Size);
                // rootQuery.OrderBy(item => item.FirstName);
                rootQuery.OrderBy(() => userAlias.FirstName);
            }
            else
            {

                rootQuery.OrderBy(item => item.FirstName);
            }

            Count = CountQuery.SingleOrDefault<int>();
            return rootQuery.List<User>();
        }
        public IEnumerable<User> GetPublishedUsers(AllAppSiteCriteria criteria, out int Count)
        {
            ISession nhibernateSession = UnitOfWork.Current.OrmSession<ISession>();
            User userAlias = null;
            ArabyAds.AdFalcon.Domain.Model.Account.Account AccountAlias = null;
            IQueryOver<User, User> rootQuery = nhibernateSession.QueryOver<User>(() => userAlias);
            ArabyAds.AdFalcon.Domain.Model.Account.UserAccounts UserAccountAlias = null;
            rootQuery.JoinAlias(() => userAlias.UserAccounts, () => UserAccountAlias)
                     .JoinAlias(() => UserAccountAlias.Account, () => AccountAlias);
           // rootQuery.JoinAlias(() => userAlias.Account, () => AccountAlias);
            var AppSitesList = QueryOver.Of<ArabyAds.AdFalcon.Domain.Model.AppSite.AppSite>().Select(M => M.Account.ID)
           .Where(M => M.Account.ID == AccountAlias.ID);
            AppSitesList.Where(m => m.IsDeleted == false);

            var PrimaryUsers = QueryOver.Of<ArabyAds.AdFalcon.Domain.Model.Account.Account>().Select(M => M.PrimaryUser.ID)
       .Where(M => M.PrimaryUser.ID == userAlias.ID);




            if (!string.IsNullOrEmpty(criteria.SubPublisherId))

            {
                ArabyAds.AdFalcon.Domain.Model.AppSite.SubAppsite SubAppsiteAlias = null;



                AppSitesList.JoinAlias(M => M.SubAppsites, () => SubAppsiteAlias);
                AppSitesList.Where(() => SubAppsiteAlias.SubPublisherId == criteria.SubPublisherId);


            }
            if (criteria.AccountId.HasValue)

            {
                rootQuery.Where(()=>UserAccountAlias.Account.ID == criteria.AccountId);
            }
            if (criteria.UserId.HasValue)

            {
                rootQuery.Where(M => M.ID == criteria.UserId);
            }
            if (criteria.DateFrom.HasValue)

            {
                AppSitesList.Where(M => M.RegistrationDate >= criteria.DateFrom);
            }
            if (criteria.DateTo.HasValue)

            {
                AppSitesList.Where(M => M.RegistrationDate <= criteria.DateTo);
            }
            if (criteria.TypeId.HasValue)

            {
                AppSitesList.Where(M => M.Type.ID == criteria.TypeId);
            }
            if (criteria.StatusId.HasValue)

            {
                AppSitesList.Where(M => M.Status.ID == criteria.StatusId);
            }
            if (!string.IsNullOrEmpty(criteria.Name))

            {
                AppSitesList.Where(M => M.Name.IsInsensitiveLike(criteria.Name.ToLower(), MatchMode.Anywhere));

            }

            if (!string.IsNullOrEmpty(criteria.Email) || !string.IsNullOrEmpty(criteria.AccountName) || !string.IsNullOrEmpty(criteria.CompanyName))

            {

                ArabyAds.AdFalcon.Domain.Model.Account.Account AccountAppSIteAlias = null;
                ArabyAds.AdFalcon.Domain.Model.Account.User UserAccountAppSIteAlias = null;
                //AppSitesList.JoinAlias(M => M.Account, () => AccountAppSIteAlias);
                //AppSitesList.JoinAlias(() => AccountAppSIteAlias.PrimaryUser, () => UserAccountAppSIteAlias);
                if (!string.IsNullOrEmpty(criteria.Email))
                    rootQuery.Where(() => userAlias.EmailAddress == criteria.Email);
                if (!string.IsNullOrEmpty(criteria.AccountName))
                    rootQuery.Where(Expression.Sql("CONCAT(UPPER(this_.FirstName), ' ', UPPER(this_.LastName)) like UPPER(?)", "%" + criteria.AccountName + "%", NHibernateUtil.String));
                //rootQuery.Where(() => userAlias.FirstName.IsInsensitiveLike(criteria.AccountName.ToLower(), MatchMode.Anywhere));
                //if (!string.IsNullOrEmpty(criteria.AccountName))
                //    rootQuery.Where(() => userAlias.LastName.IsInsensitiveLike(criteria.AccountName.ToLower(), MatchMode.Anywhere));
                if (!string.IsNullOrEmpty(criteria.CompanyName))
                    rootQuery.Where(() => userAlias.Company.IsInsensitiveLike(criteria.CompanyName.ToLower(), MatchMode.Anywhere));
            }
            rootQuery.WithSubquery.WhereExists(AppSitesList);
            rootQuery.WithSubquery.WhereExists(PrimaryUsers);

            var CountQuery = rootQuery.ToRowCountQuery();
            rootQuery.OrderBy(() => userAlias.FirstName);
            if (criteria.Page >= 0)
            {
                var pageIndex = criteria.Page - 1;
                //rootQuery.OrderBy(item => item.FirstName);
                rootQuery.Skip(pageIndex * criteria.Size)
                                         .Take(criteria.Size);
                rootQuery.OrderBy(() => userAlias.FirstName).Asc();
            }
            else
            {

                rootQuery.OrderBy(item => item.FirstName);
            }

            Count = CountQuery.SingleOrDefault<int>();
            return rootQuery.List<User>();
        }
        public IEnumerable<User> QueryByCratiriaForUsers(Domain.Repositories.Account.UserCriteriaBase criteria, out int Count)
        {

            ISession nhibernateSession = UnitOfWork.Current.OrmSession<ISession>();
            User userAlias = null;
            IQueryOver<User, User> rootQuery = nhibernateSession.QueryOver<User>(() => userAlias);
            ArabyAds.AdFalcon.Domain.Model.Account.Account AccountAlias = null;

            ArabyAds.AdFalcon.Domain.Model.Account.UserAccounts UserAccountAlias = null;
            rootQuery.JoinAlias(() => userAlias.UserAccounts, () => UserAccountAlias)
                     .JoinAlias(() => UserAccountAlias.Account, () => AccountAlias);

            criteria.Name = string.IsNullOrEmpty(criteria.Name) ? string.Empty : criteria.Name.Trim();
            criteria.CompanyName = string.IsNullOrEmpty(criteria.CompanyName) ? string.Empty : criteria.CompanyName.Trim();
            criteria.Email = string.IsNullOrEmpty(criteria.Email) ? string.Empty : criteria.Email.Trim();

            if (!string.IsNullOrEmpty(criteria.Email))
            {
                rootQuery.Where(M => M.EmailAddress.IsInsensitiveLike(criteria.Email.ToLower(), MatchMode.Anywhere));

            }


            if (!string.IsNullOrEmpty(criteria.CompanyName))
            {
                rootQuery.Where(M => M.Company.IsInsensitiveLike(criteria.CompanyName.ToLower(), MatchMode.Anywhere));

            }

            if (!string.IsNullOrEmpty(criteria.Name))
            {
                // rootQuery.Where(M =>( M.FirstName+" "+M.LastName).IsInsensitiveLike(criteria.Name, MatchMode.Anywhere));


                rootQuery.Where(Expression.Sql("CONCAT(UPPER(this_.FirstName), ' ', UPPER(this_.LastName)) like UPPER(?)", "%" + criteria.Name + "%", NHibernateUtil.String));
                //rootQuery.Where(M => M.LastName.IsInsensitiveLike(criteria.Name, MatchMode.Anywhere));
            }
            if (criteria.AccountId.HasValue)
            {
                rootQuery.Where(() => UserAccountAlias.Account.ID == criteria.AccountId.Value);

            }
            if (criteria.hideCurrentUser)
            {

                rootQuery.Where(()=> UserAccountAlias.User.ID != OperationContext.Current.UserInfo<AdFalconUserInfo>().UserId.Value);

            }
            if (criteria.hideNonPrimary)
            {
                var subqueryPrimaryUser = QueryOver.Of<ArabyAds.AdFalcon.Domain.Model.Account.Account>()
    .Where(gm => gm.PrimaryUser.ID == userAlias.ID)
    .Select(gm => gm.ID);
                rootQuery.WithSubquery.WhereExists(subqueryPrimaryUser);

            }
            if (criteria.Role>0)
            {
                var subqueryRoleUser = QueryOver.Of<ArabyAds.AdFalcon.Domain.Model.Account.Account>()
    .Where(gm => gm.AccountRole== (AccountRole)criteria.Role)
     .Where(gm => gm.PrimaryUser.ID == userAlias.ID)
    .Select(gm => gm.ID);
                rootQuery.WithSubquery.WhereExists(subqueryRoleUser);

            }

            if (criteria.publisherUsers)
            {
          
               // rootQuery.JoinAlias(() => userAlias.Account, () => AccountAlias);

                var appSitesAccounts = QueryOver.Of<ArabyAds.AdFalcon.Domain.Model.AppSite.AppSite>()
    .Where(gm => gm.Account.ID == AccountAlias.ID)
    .Select(gm => gm.ID);
                rootQuery.WithSubquery.WhereExists(appSitesAccounts);

            }



            var CountQuery = rootQuery.ToRowCountQuery();
            if (criteria.Page >= 0)
            {
                var pageIndex = criteria.Page;
                rootQuery.OrderBy(item => item.ID);
                rootQuery.Skip(pageIndex * criteria.Size)
                                         .Take(criteria.Size);
                rootQuery.OrderBy(item => item.FirstName);
            }
            else
            {

                rootQuery.OrderBy(item => item.ID);
            }

            Count = CountQuery.SingleOrDefault<int>();
            return rootQuery.List<User>();


        }
        public int GetUserAccountIdByEmail(string emailAddress)
        {
            ISession nhibernateSession = UnitOfWork.Current.OrmSession<ISession>();
            User userAlias = null;
            UserAccounts userAccountsAlias = null;
            IQueryOver<User, User> rootQuery = nhibernateSession.QueryOver<User>(() => userAlias);
            rootQuery.Where(M => M.EmailAddress.IsInsensitiveLike(emailAddress.ToLower()));

            rootQuery.Select(M => M.ID);
            var userId= rootQuery.SingleOrDefault<int>();

            if (userId > 0)
            {
                IQueryOver<UserAccounts, UserAccounts> rootQueryUserAccounts = nhibernateSession.QueryOver<UserAccounts>(() => userAccountsAlias);
                rootQueryUserAccounts.Where(M => M.User.ID == userId);

                rootQueryUserAccounts.Select(M => M.Account.ID);
                var accountIds = rootQueryUserAccounts.List<int>();
                if (accountIds != null && accountIds.Count > 0)
                    return accountIds[0];
            }
            return 0;

        }

      /*  public ArabyAds.AdFalcon.Domain.Model.Account.Account GetUserAccountByEmail(string emailAddress)
        {
            ISession nhibernateSession = UnitOfWork.Current.OrmSession<ISession>();
            User userAlias = null;
           
            IQueryOver<User, User> rootQuery = nhibernateSession.QueryOver<User>(() => userAlias);
            rootQuery.Where(M => M.EmailAddress.IsInsensitiveLike(emailAddress.ToLower()));

            rootQuery.Select(M=>M.Account);
            return rootQuery.SingleOrDefault<ArabyAds.AdFalcon.Domain.Model.Account.Account>();
        } */
    }
}
