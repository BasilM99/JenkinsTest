using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArabyAds.Framework.Persistence;
using ArabyAds.AdFalcon.Domain.Model.Core;
using ArabyAds.AdFalcon.Domain.Repositories.Core;

namespace ArabyAds.AdFalcon.Persistence.Repositories.Core
{
    public class LookupRepository : RepositoryBase<ManagedLookupBase, int>, ILookupRepository
    {
        public LookupRepository(RepositoryImplBase<ManagedLookupBase, int> repository)
            : base(repository)
        {

        }
    }
}
