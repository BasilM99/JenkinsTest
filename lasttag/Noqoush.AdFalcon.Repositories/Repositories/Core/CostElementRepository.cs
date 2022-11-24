using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Noqoush.AdFalcon.Domain.Model.Core.CostElement;
using Noqoush.Framework.Persistence;
using Noqoush.AdFalcon.Domain.Model.Core;
using Noqoush.AdFalcon.Domain.Repositories.Core;

namespace Noqoush.AdFalcon.Persistence.Repositories.Core
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
