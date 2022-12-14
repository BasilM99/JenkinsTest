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
    public class CreativeUnitGroupRepository : RepositoryBase<CreativeUnitGroup, int>, ICreativeUnitGroupRepository
    {
        public CreativeUnitGroupRepository(RepositoryImplBase<CreativeUnitGroup, int> repository)
            : base(repository)
        {


        }
    }
}
