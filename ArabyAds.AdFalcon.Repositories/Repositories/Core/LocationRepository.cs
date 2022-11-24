using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArabyAds.AdFalcon.Domain.Model.Core;
using ArabyAds.AdFalcon.Domain.Repositories.Core;
using ArabyAds.Framework.Persistence;

namespace ArabyAds.AdFalcon.Persistence.Repositories
{
    public class LocationRepository : RepositoryBase<LocationBase, int>, ILocationRepository
    {
        public LocationRepository(RepositoryImplBase<LocationBase, int> repository)
            : base(repository)
        {


        }
    }
}
