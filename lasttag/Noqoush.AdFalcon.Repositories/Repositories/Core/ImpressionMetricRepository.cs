using Noqoush.AdFalcon.Domain.Model.Core;
using Noqoush.AdFalcon.Domain.Repositories.Campaign;
using Noqoush.Framework.Persistence;
using Noqoush.AdFalcon.Domain.Repositories.Core;

namespace Noqoush.AdFalcon.Persistence.Repositories.Core
{
    public class ImpressionMetricRepository : RepositoryBase<ImpressionMetric, int>, IImpressionMetricRepository
    {
        public ImpressionMetricRepository(RepositoryImplBase<ImpressionMetric, int> repository)
            : base(repository)
        {


        }
    }
}
