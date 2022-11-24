using ArabyAds.AdFalcon.Domain.Model.Core;
using ArabyAds.AdFalcon.Domain.Repositories.Campaign;
using ArabyAds.Framework.Persistence;
using ArabyAds.AdFalcon.Domain.Repositories.Core;

namespace ArabyAds.AdFalcon.Persistence.Repositories.Core
{
    public class ImpressionMetricRepository : RepositoryBase<ImpressionMetric, int>, IImpressionMetricRepository
    {
        public ImpressionMetricRepository(RepositoryImplBase<ImpressionMetric, int> repository)
            : base(repository)
        {


        }
    }
}
