using NHibernate;
using Noqoush.AdFalcon.Domain.Model.Core;
using Noqoush.AdFalcon.Domain.Repositories.Core;
using Noqoush.Framework.Persistence;

namespace Noqoush.AdFalcon.Persistence.Repositories.Core
{
    public class metriceColumnRepository : RepositoryBase<metriceColumn, int>, ImetriceColumnRepository
    {
        public metriceColumnRepository(RepositoryImplBase<metriceColumn, int> repository)
            : base(repository)
        {


        }

        public int GetColumnId(string AppFieldName, bool Publisher)
        {

            var nhibernateSession = UnitOfWork.Current.OrmSession<ISession>();
            int id = 0;
            if (Publisher)
            {
                id = nhibernateSession.QueryOver<metriceColumn>()
               .Where(x => x.AppFieldName == AppFieldName && x.Publisher == Publisher && x.Advertiser == false)
               .Select(x => x.Id)
               .SingleOrDefault<int>();
            }

            else
            {
                id = nhibernateSession.QueryOver<metriceColumn>()
               .Where(x => x.AppFieldName == AppFieldName && x.Advertiser == true && x.Publisher == false)
               .Select(x => x.Id)
               .SingleOrDefault<int>();

            }
            return id;
        }
    }
}
