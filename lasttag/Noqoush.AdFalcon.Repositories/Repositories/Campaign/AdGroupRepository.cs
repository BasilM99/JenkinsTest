using Noqoush.AdFalcon.Domain.Model.Campaign;
using Noqoush.AdFalcon.Domain.Repositories.Campaign;
using Noqoush.Framework.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate;
namespace Noqoush.AdFalcon.Persistence.Repositories.Campaign
{
    class AdGroupRepository : RepositoryBase<AdGroup, int>, IAdGroupRepository
    {
        public AdGroupRepository(RepositoryImplBase<AdGroup, int> repository)
            : base(repository)
        {
        }
        public string GetObjectName(int Id)
        {

            ISession nhibernateSession = UnitOfWork.Current.OrmSession<ISession>();

            IQueryOver<AdGroup, AdGroup> rootQuery = nhibernateSession.QueryOver<AdGroup>();

            //joins


            rootQuery.Where(
                        p => p.ID == Id);


            return rootQuery.Select(M => M.Name).SingleOrDefault<string>();


        }

    }
}
