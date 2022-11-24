using ArabyAds.AdFalcon.Domain.Model.AppSite;
using ArabyAds.AdFalcon.Domain.Model.Campaign;
using ArabyAds.AdFalcon.Domain.Repositories.Campaign;
using ArabyAds.Framework.Persistence;

namespace ArabyAds.AdFalcon.Persistence.Repositories.Campaign
{
    public class MetricVendorRepository : RepositoryBase<MetricVendor, int>, IMetricVendorRepository
    {
        public MetricVendorRepository(RepositoryImplBase<MetricVendor, int> repository)
            : base(repository)
        {
        }
    }
}
