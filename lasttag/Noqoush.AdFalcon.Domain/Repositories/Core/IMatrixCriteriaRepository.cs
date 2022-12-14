using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Noqoush.Framework.Persistence;
using Noqoush.AdFalcon.Domain.Model.Core;

namespace Noqoush.AdFalcon.Domain.Repositories.Core
{
    public interface ImetriceColumnReportCriteriaRepository : IKeyedRepository<metriceColumnReportCriteria, int>
    {
        List<int> GetmetriceColumnsForTemplate(int TemplateId);
    }

}
