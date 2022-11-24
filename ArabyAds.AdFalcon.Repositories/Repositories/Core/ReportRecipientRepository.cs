using ArabyAds.AdFalcon.Domain.Model.Core;
using ArabyAds.AdFalcon.Domain.Repositories.Core;
using ArabyAds.Framework.Persistence;

namespace ArabyAds.AdFalcon.Persistence.Repositories.Core
{
    public class ReportRecipientRepository : RepositoryBase<ReportRecipient, int>, IReportRecipientRepository
    {
        public ReportRecipientRepository(RepositoryImplBase<ReportRecipient, int> repository)
            : base(repository)
        {


        }
    }
}
