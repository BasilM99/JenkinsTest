using Noqoush.AdFalcon.Domain.Model.Account.DPP;
using Noqoush.AdFalcon.Domain.Repositories.Account.DPP;
using Noqoush.Framework.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Noqoush.AdFalcon.Persistence.Repositories.Account.DPP
{
    public class ImpressionLogRepository : RepositoryBase<ImpressionLog, int>, IImpressionLogRepository
    {
        public ImpressionLogRepository(RepositoryImplBase<ImpressionLog, int> repository)
            : base(repository)
        {
        }
    }
}
