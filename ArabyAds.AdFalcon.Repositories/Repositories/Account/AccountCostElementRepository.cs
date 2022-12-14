using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArabyAds.Framework.Persistence;
using ArabyAds.AdFalcon.Domain.Repositories;
using ArabyAds.AdFalcon.Domain.Model.Account;
using ArabyAds.AdFalcon.Domain.Model.Core;
using ArabyAds.Framework;
using ArabyAds.AdFalcon.Common.UserInfo;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Transform;
using NHibernate.Criterion.Lambda;
using ArabyAds.AdFalcon.Domain.Repositories.Account;
using ArabyAds.AdFalcon.Domain.Model.Core.CostElement;

namespace ArabyAds.AdFalcon.Persistence.Repositories
{
    public class AccountCostElementRepository : RepositoryBase<AccountCostElement, int>, IAccountCostElementRepository
    {
        public AccountCostElementRepository(RepositoryImplBase<AccountCostElement, int> repository)
            : base(repository)
        {
        }
       
        public List<AccountCostElement> GetAccountCostElements(int accountId)
        {




            ISession nhibernateSession = UnitOfWork.Current.OrmSession<ISession>();
            AccountCostElement AccountCostElementAlias = null;
            IQueryOver<AccountCostElement, AccountCostElement> rootQuery = nhibernateSession.QueryOver<AccountCostElement>(() => AccountCostElementAlias);
            rootQuery.Where(M => M.Account.ID == accountId);
            rootQuery.Where(M=>M.Enabled==true);
           // rootQuery.Select(x => x.CostElement);


            IList<AccountCostElement> list = rootQuery.List<AccountCostElement>();
            return list.ToList();



        }

    }


    public class AccountFeeRepository : RepositoryBase<AccountFee, int>, IAccountFeeRepository
    {
        public AccountFeeRepository(RepositoryImplBase<AccountFee, int> repository)
            : base(repository)
        {
        }

        public List<AccountFee> GetAccountFees(int accountId)
        {




            ISession nhibernateSession = UnitOfWork.Current.OrmSession<ISession>();
            AccountFee AccountCostElementAlias = null;
            IQueryOver<AccountFee, AccountFee> rootQuery = nhibernateSession.QueryOver<AccountFee>(() => AccountCostElementAlias);
            rootQuery.Where(M => M.Account.ID == accountId);
            rootQuery.Where(M => M.Enabled == true);
            // rootQuery.Select(x => x.CostElement);


            IList<AccountFee> list = rootQuery.List<AccountFee>();
            return list.ToList();



        }

    }
}
