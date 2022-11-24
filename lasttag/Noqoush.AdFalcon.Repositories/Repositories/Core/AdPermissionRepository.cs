using Noqoush.AdFalcon.Domain.Model.Account;
using Noqoush.AdFalcon.Domain.Model.Core;
using Noqoush.AdFalcon.Domain.Repositories;
using Noqoush.AdFalcon.Domain.Repositories.Account;
using Noqoush.AdFalcon.Domain.Repositories.Core;
using Noqoush.Framework.Persistence;
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
using Noqoush.AdFalcon.Domain.Common.Model.Core;

namespace Noqoush.AdFalcon.Persistence.Repositories.Core
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
