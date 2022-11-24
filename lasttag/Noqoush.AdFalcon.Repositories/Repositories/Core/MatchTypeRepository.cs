using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Noqoush.AdFalcon.Domain.Model.AppSite;
using Noqoush.AdFalcon.Domain.Repositories.Core;
using Noqoush.Framework.Persistence;

namespace Noqoush.AdFalcon.Persistence.Repositories.Core
{
    public class MatchTypeRepository : RepositoryBase<MatchType, int>, IMatchTypeRepository
    {
        public MatchTypeRepository(RepositoryImplBase<MatchType, int> repository)
            : base(repository)
        {


        }
    }
    
}
