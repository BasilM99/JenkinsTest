using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Noqoush.AdFalcon.Domain.Repositories.Core;
using Noqoush.AdFalcon.Domain.Model.Core;
using Noqoush.Framework.Persistence;

namespace Noqoush.AdFalcon.Persistence.Repositories.Core
{
    public class MetricRepository : RepositoryBase<Metric, int>, IMetricRepository
    {
        public MetricRepository(RepositoryImplBase<Metric, int> repository)
            : base(repository)
        {

        }
    }
}
