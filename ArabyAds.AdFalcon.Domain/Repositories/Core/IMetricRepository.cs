using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArabyAds.Framework.Persistence;
using ArabyAds.AdFalcon.Domain.Model.Core;

namespace ArabyAds.AdFalcon.Domain.Repositories.Core
{
    public interface IMetricRepository : IKeyedRepository<Metric, int>
    {
    }
}
