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
using ArabyAds.AdFalcon.Domain.Common.Model.Core;

namespace ArabyAds.AdFalcon.Persistence.Repositories
{
    public class AccountPortalPermissionsRepository : RepositoryBase<AccountPortalPermissions, int>, IAccountPortalPermissionsRepository
    {
        public AccountPortalPermissionsRepository(RepositoryImplBase<AccountPortalPermissions, int> repository)
            : base(repository)
        {
        }

        public List<PortalPermision> GetAccountAdPermissions(int accountId)
        {
            



                ISession nhibernateSession = UnitOfWork.Current.OrmSession<ISession>();
            AccountPortalPermissions AccountPortalPermissionsAlias = null;
                IQueryOver<AccountPortalPermissions, AccountPortalPermissions> rootQuery = nhibernateSession.QueryOver<AccountPortalPermissions>(() => AccountPortalPermissionsAlias);

            rootQuery.Where(M=>M.Account.ID== accountId);
            rootQuery.Select(x => x.Permission);

                IList<PortalPermision> list = rootQuery.List<PortalPermision>();
                return list.ToList();

        

        }


        public bool checkAdPermissions(PortalPermissionsCode Code)
        {
            bool result = false;
            //To Be Refactor
            if (OperationContext.Current.CurrentPrincipal.IsInRole("Administrator") || OperationContext.Current.CurrentPrincipal.IsInRole("AdOps")|| OperationContext.Current.CurrentPrincipal.IsInRole("AccountManager"))
            { return true; }
            if (OperationContext.Current.UserInfo<AdFalconUserInfo>().Permissions!=null)
             result = OperationContext.Current.UserInfo<AdFalconUserInfo>().Permissions.ToList().Where(x => x == (int)Code).Count() > 0;
           

            return result;
        }
    }
}
