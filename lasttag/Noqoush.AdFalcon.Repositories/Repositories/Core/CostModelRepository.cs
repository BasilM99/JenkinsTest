using Noqoush.AdFalcon.Domain.Model.Core;
using Noqoush.AdFalcon.Domain.Repositories.Core;
using Noqoush.Framework.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Noqoush.AdFalcon.Persistence.Repositories.Core
{
    public class CostModelRepository : RepositoryBase<CostModel, int>, ICostModelRepository
    {
        public CostModelRepository(RepositoryImplBase<CostModel, int> repository)
            : base(repository)
        {

        }
    }
}
