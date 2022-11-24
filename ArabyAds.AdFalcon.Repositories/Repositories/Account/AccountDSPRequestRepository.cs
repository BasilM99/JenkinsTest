using ArabyAds.AdFalcon.Domain.Model.Account;
using ArabyAds.AdFalcon.Domain.Model.Account.Discount;
using ArabyAds.AdFalcon.Domain.Repositories;
using ArabyAds.AdFalcon.Domain.Repositories.Account;
using ArabyAds.AdFalcon.Domain.Repositories.Core;
using ArabyAds.Framework.Persistence;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Transform;
using System.Linq;
using NHibernate.Criterion.Lambda;
using System.Collections.Generic;
using ArabyAds.AdFalcon.Domain.Common.Model.Account;

namespace ArabyAds.AdFalcon.Persistence.Repositories.Account
{
   

    public class AccountDSPRequestRepository : RepositoryBase<AccountDSPRequest, int>, IAccountDSPRequestRepository
    {
        public AccountDSPRequestRepository(RepositoryImplBase<AccountDSPRequest, int> repository)
            : base(repository)
        {
        }


        public IEnumerable<AccountDSPRequest> QueryByCratiriaForAccountDSPRequests(Domain.Repositories.Account.UserCriteriaBase criteria, out int Count)
        {

            ISession nhibernateSession = UnitOfWork.Current.OrmSession<ISession>();
            AccountDSPRequest userAlias = null;
            IQueryOver<AccountDSPRequest, AccountDSPRequest> rootQuery = nhibernateSession.QueryOver<AccountDSPRequest>(() => userAlias);
            criteria.Name = string.IsNullOrEmpty(criteria.Name) ? string.Empty : criteria.Name.Trim();
            criteria.CompanyName = string.IsNullOrEmpty(criteria.CompanyName) ? string.Empty : criteria.CompanyName.Trim();
            criteria.Email = string.IsNullOrEmpty(criteria.Email) ? string.Empty : criteria.Email.Trim();

            if (criteria.StatusId>0)
            {
                AccountDSPRequestStatus statusDSP = (AccountDSPRequestStatus)criteria.StatusId;
                
                rootQuery.Where(M => M.Status== statusDSP);

            }
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
                rootQuery.Where(M => M.Account.ID == criteria.AccountId.Value);

            }
            else {

                rootQuery.Where(M => M.Account==null);
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
            return rootQuery.List<AccountDSPRequest>();


        }
        public bool CheckEmailAddress(string emailAddress)
        {
           
            ISession nhibernateSession = UnitOfWork.Current.OrmSession<ISession>();

            IQueryOver<AccountDSPRequest, AccountDSPRequest> rootQuery = nhibernateSession.QueryOver<AccountDSPRequest>();

            rootQuery.Where(M => M.EmailAddress.IsInsensitiveLike(emailAddress.ToLower()));
            var results= rootQuery.List();
            var exist = false;
            if(results!=null)
             exist = results.Count > 0 ? true : false;
            return exist;

        }
        public bool CheckEmailAddressInvited(string emailAddress)
        {

            ISession nhibernateSession = UnitOfWork.Current.OrmSession<ISession>();

            IQueryOver<AccountInvitation, AccountInvitation> rootQuery = nhibernateSession.QueryOver<AccountInvitation>();

            rootQuery.Where(M => M.EmailAddress.IsInsensitiveLike(emailAddress.ToLower()));
            rootQuery.Where(M=>M.IsAccepted==true);

            var results = rootQuery.List();

            var exist = false;
            if (results != null)
            {
                foreach (var item in results)
                {
                    if (item.Account!=null)
                    {
                        if (item.Account.AccountRole== AccountRole.DSP)
                        {
                            return true;

                        }
                    }

                }
            }
            return exist;

        }
        public AccountDSPRequest GetByEmailAddress(string emailAddress)
        {

            ISession nhibernateSession = UnitOfWork.Current.OrmSession<ISession>();

            IQueryOver<AccountDSPRequest, AccountDSPRequest> rootQuery = nhibernateSession.QueryOver<AccountDSPRequest>();

            rootQuery.Where(M => M.EmailAddress.IsInsensitiveLike(emailAddress.ToLower()));
            var results = rootQuery.SingleOrDefault();

            return results;

        }
        public AccountDSPRequest GetByEmailAddressApproved(string emailAddress)
        {

            ISession nhibernateSession = UnitOfWork.Current.OrmSession<ISession>();

            IQueryOver<AccountDSPRequest, AccountDSPRequest> rootQuery = nhibernateSession.QueryOver<AccountDSPRequest>();

            rootQuery.Where(M => M.EmailAddress.IsInsensitiveLike(emailAddress.ToLower()));
            rootQuery.Where(M=>M.ActionDate!=null && M.Status== AccountDSPRequestStatus.Approved);
            var results = rootQuery.SingleOrDefault();

            return results;

        }
        public AccountDSPRequest GetByRequestCode(string requestCode)
        {

            ISession nhibernateSession = UnitOfWork.Current.OrmSession<ISession>();

            IQueryOver<AccountDSPRequest, AccountDSPRequest> rootQuery = nhibernateSession.QueryOver<AccountDSPRequest>();

            rootQuery.Where(M => M.RequestCode.IsInsensitiveLike(requestCode.ToLower()));

            rootQuery.Where(M=>M.Account==null);

            var results = rootQuery.SingleOrDefault();

            return results;

        }
    }
}
