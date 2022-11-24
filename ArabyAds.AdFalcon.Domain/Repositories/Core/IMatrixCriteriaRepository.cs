using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArabyAds.Framework.Persistence;
using ArabyAds.AdFalcon.Domain.Model.Core;

namespace ArabyAds.AdFalcon.Domain.Repositories.Core
{
    public interface ImetriceColumnReportCriteriaRepository : IKeyedRepository<metriceColumnReportCriteria, int>
    {
        List<int> GetmetriceColumnsForTemplate(int TemplateId);
    }

}
