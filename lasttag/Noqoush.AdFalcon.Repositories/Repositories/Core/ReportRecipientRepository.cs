using Noqoush.AdFalcon.Domain.Model.Core;
using Noqoush.AdFalcon.Domain.Repositories.Core;
using Noqoush.Framework.Persistence;

namespace Noqoush.AdFalcon.Persistence.Repositories.Core
{
    public class ReportRecipientRepository : RepositoryBase<ReportRecipient, int>, IReportRecipientRepository
    {
        public ReportRecipientRepository(RepositoryImplBase<ReportRecipient, int> repository)
            : base(repository)
        {


        }
    }
}
