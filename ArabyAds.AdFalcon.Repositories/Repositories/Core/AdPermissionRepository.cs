using ArabyAds.AdFalcon.Domain.Model.Account;
using ArabyAds.AdFalcon.Domain.Model.Core;
using ArabyAds.AdFalcon.Domain.Repositories;
using ArabyAds.AdFalcon.Domain.Repositories.Account;
using ArabyAds.AdFalcon.Domain.Repositories.Core;
using ArabyAds.Framework.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Transform;
using NHibernate.Criterion.Lambda;
using System.Linq;
using System.Collections.Generic;
using ArabyAds.AdFalcon.Domain.Common.Model.Core;

namespace ArabyAds.AdFalcon.Persistence.Repositories.Core
{
    public class PortalPermisionRepository : RepositoryBase<PortalPermision, int>, IPortalPermisionRepository
    {
        public PortalPermisionRepository(RepositoryImplBase<PortalPermision, int> repository)
            : base(repository)
        {
        }


        public PortalPermision GetByCode(PortalPermissionsCode Code )
        {

            ISession nhibernateSession = UnitOfWork.Current.OrmSession<ISession>();
            PortalPermision PortalPermisionAlias = null;
            IQueryOver<PortalPermision, PortalPermision> rootQuery = nhibernateSession.QueryOver<PortalPermision>(() => PortalPermisionAlias);

            rootQuery.Where(M=>M.Code== Code);
           return  rootQuery.SingleOrDefault();

        }
    }
}
