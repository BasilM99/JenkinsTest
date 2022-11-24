using ArabyAds.AdFalcon.Domain.Model.Core;
using ArabyAds.AdFalcon.Domain.Repositories.Core;
using ArabyAds.Framework.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArabyAds.AdFalcon.Persistence.Repositories.Core
{
    public class CostModelRepository : RepositoryBase<CostModel, int>, ICostModelRepository
    {
        public CostModelRepository(RepositoryImplBase<CostModel, int> repository)
            : base(repository)
        {

        }
    }
}
