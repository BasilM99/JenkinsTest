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
using Noqoush.AdFalcon.Domain.Model.Account;
using Noqoush.Framework;
using Noqoush.AdFalcon.Common.UserInfo;

using System.Collections.Generic;
using System;

namespace Noqoush.AdFalcon.Persistence.Repositories.Core
{
    public class BusinessPartnerRepository : RepositoryBase<BusinessPartner, int>, IBusinessPartnerRepository
    {
        public BusinessPartnerRepository(RepositoryImplBase<BusinessPartner, int> repository)
            : base(repository)
        {


        }
        public string GetObjectName(int Id)
        {

            ISession nhibernateSession = UnitOfWork.Current.OrmSession<ISession>();

            IQueryOver<BusinessPartner, BusinessPartner> rootQuery = nhibernateSession.QueryOver<BusinessPartner>();

            //joins


            rootQuery.Where(
                        p => p.ID == Id);


            return rootQuery.Select(M => M.Name).SingleOrDefault<string>();


        }
        public IList<BusinessPartner> GetBusinessPartnerByAccountIds(int[] accounts)
        {
            ISession nhibernateSession = UnitOfWork.Current.OrmSession<NHibernate.ISession>();
            BusinessPartner BusinessPartnerAlias = null;


            IQueryOver<BusinessPartner, BusinessPartner> rootQuery = nhibernateSession.QueryOver<BusinessPartner>(() => BusinessPartnerAlias);

            rootQuery.Where(M => M.Account.ID.IsIn(accounts));

            return rootQuery.List<BusinessPartner>();
        }

        public int getAccount(int id)
        {

            ISession nhibernateSession = UnitOfWork.Current.OrmSession<ISession>();


            int accountId = nhibernateSession.QueryOver<BusinessPartner>().Where(x => x.ID == id).Select(
            Projections.Property<BusinessPartner>(x => x.Account.ID)).SingleOrDefault<int>();

            return accountId;
        }
    }
}
