using ArabyAds.AdFalcon.Domain.Model.Account.DPP;
using ArabyAds.AdFalcon.Domain.Repositories.Account.DPP;
using ArabyAds.Framework.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArabyAds.AdFalcon.Persistence.Repositories.Account.DPP
{
    public class ImpressionLogRepository : RepositoryBase<ImpressionLog, int>, IImpressionLogRepository
    {
        public ImpressionLogRepository(RepositoryImplBase<ImpressionLog, int> repository)
            : base(repository)
        {
        }
    }
}
