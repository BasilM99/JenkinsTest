
using NHibernate;
using Noqoush.AdFalcon.Domain.Model.Core;
using Noqoush.AdFalcon.Domain.Repositories.Core;
using Noqoush.Framework.Persistence;

namespace Noqoush.AdFalcon.Persistence.Repositories.Core
{
    public class ReportSchedulerRepository : RepositoryBase<ReportScheduler, int>, IReportSchedulerRepository
    {
        public ReportSchedulerRepository(RepositoryImplBase<ReportScheduler, int> repository)
            : base(repository)
        {
        }

        public string GetObjectName(int Id)
        {

            ISession nhibernateSession = UnitOfWork.Current.OrmSession<ISession>();

            IQueryOver<ReportScheduler, ReportScheduler> rootQuery = nhibernateSession.QueryOver<ReportScheduler>();
            
            //joins


            rootQuery.Where(
                        p => p.ID== Id);


            return rootQuery.Select(M => M.Name).SingleOrDefault<string>();


        }

    
    }
}
