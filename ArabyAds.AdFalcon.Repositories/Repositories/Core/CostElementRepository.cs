using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArabyAds.AdFalcon.Domain.Model.Core.CostElement;
using ArabyAds.Framework.Persistence;
using ArabyAds.AdFalcon.Domain.Model.Core;
using ArabyAds.AdFalcon.Domain.Repositories.Core;

namespace ArabyAds.AdFalcon.Persistence.Repositories.Core
{
    public class CostElementRepository : RepositoryBase<CostElement, int>, ICostElementRepository
    {
        public CostElementRepository(RepositoryImplBase<CostElement, int> repository)
            : base(repository)
        {

        }
    }

    public class FeeRepository : RepositoryBase<Fee, int>, IFeeRepository
    {
        public FeeRepository(RepositoryImplBase<Fee, int> repository)
            : base(repository)
        {

        }
    }
}
