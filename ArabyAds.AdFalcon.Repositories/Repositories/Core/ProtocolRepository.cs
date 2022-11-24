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
using NHibernate.Criterion.Lambda;
using NHibernate.SqlCommand;
using NHibernate.Transform;
using NHibernate.Linq;
namespace ArabyAds.AdFalcon.Persistence.Repositories.Core
{
    public class ProtocolRepository : RepositoryBase<Protocol, int>, IProtocolRepository
    {
        public ProtocolRepository(RepositoryImplBase<Protocol, int> repository)
            : base(repository)
        {
        }
        public Protocol GetByCode(int CodeId)
        {

            ISession nhibernateSession = UnitOfWork.Current.OrmSession<ISession>();

            IQueryOver<Protocol, Protocol> rootQuery = nhibernateSession.QueryOver<Protocol>();

            //joins


            rootQuery.Where(
                        p => p.Code == CodeId);


            return rootQuery.SingleOrDefault<Protocol>();


        }
    }
}
