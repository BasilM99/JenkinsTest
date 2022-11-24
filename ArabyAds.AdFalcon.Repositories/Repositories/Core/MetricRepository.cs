using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArabyAds.AdFalcon.Domain.Repositories.Core;
using ArabyAds.AdFalcon.Domain.Model.Core;
using ArabyAds.Framework.Persistence;

namespace ArabyAds.AdFalcon.Persistence.Repositories.Core
{
    public class MetricRepository : RepositoryBase<Metric, int>, IMetricRepository
    {
        public MetricRepository(RepositoryImplBase<Metric, int> repository)
            : base(repository)
        {

        }
    }
}
