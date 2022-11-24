using Noqoush.AdFalcon.Domain.Model.AppSite;
using Noqoush.AdFalcon.Domain.Model.Campaign;
using Noqoush.AdFalcon.Domain.Repositories.Campaign;
using Noqoush.Framework.Persistence;

namespace Noqoush.AdFalcon.Persistence.Repositories.Campaign
{
    public class MetricVendorRepository : RepositoryBase<MetricVendor, int>, IMetricVendorRepository
    {
        public MetricVendorRepository(RepositoryImplBase<MetricVendor, int> repository)
            : base(repository)
        {
        }
    }
}
