using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Noqoush.AdFalcon.Domain.Model.Core;
using Noqoush.AdFalcon.Domain.Repositories.Core;
using Noqoush.Framework.Persistence;

namespace Noqoush.AdFalcon.Persistence.Repositories
{
    public class LocationRepository : RepositoryBase<LocationBase, int>, ILocationRepository
    {
        public LocationRepository(RepositoryImplBase<LocationBase, int> repository)
            : base(repository)
        {


        }
    }
}
