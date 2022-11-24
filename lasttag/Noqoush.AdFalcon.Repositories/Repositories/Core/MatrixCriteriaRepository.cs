using NHibernate;
using Noqoush.AdFalcon.Domain.Model.Core;
using Noqoush.AdFalcon.Domain.Repositories.Core;
using Noqoush.Framework.Persistence;
using System.Collections.Generic;
using System.Linq;

namespace Noqoush.AdFalcon.Persistence.Repositories.Core
{
    public class metriceColumnReportCriteriaRepository : RepositoryBase<metriceColumnReportCriteria, int>, ImetriceColumnReportCriteriaRepository
    {
        public metriceColumnReportCriteriaRepository(RepositoryImplBase<metriceColumnReportCriteria, int> repository)
            : base(repository)
        {


        }

        public List<int> GetmetriceColumnsForTemplate(int TemplateId)
        {

            var nhibernateSession = UnitOfWork.Current.OrmSession<ISession>();

            IList<int> ids = nhibernateSession.QueryOver<metriceColumnReportCriteria>()
          .Where(x => x.ReportCriteria.ID == TemplateId)
          .Select(x => x.metriceColumn.Id).List<int>();

            return ids.ToList();

            //ISession nhibernateSession = UnitOfWork.Current.OrmSession<ISession>();
            //metriceColumnReportCriteria metriceColumnReportCriteriaAlias = null;
            //IQueryOver<metriceColumnReportCriteria, metriceColumnReportCriteria> rootQuery = nhibernateSession.QueryOver<metriceColumnReportCriteria>(() => metriceColumnReportCriteriaAlias);

            //rootQuery.Where(x => x.ReportCriteria.ID == TemplateId);
            //rootQuery.Select(x => x.metriceColumn.Id);

            //IList<int> list = rootQuery.List<int>();
            //return list.ToList();
        }

    }
}
