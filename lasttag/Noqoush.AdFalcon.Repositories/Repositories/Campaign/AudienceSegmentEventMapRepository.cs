using Noqoush.AdFalcon.Domain.Model.Campaign;
using Noqoush.AdFalcon.Domain.Repositories.Campaign;
using Noqoush.AdFalcon.Domain.Repositories.Core;
using Noqoush.Framework.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Noqoush.AdFalcon.Persistence.Repositories.Campaign
{
   

    public class AudienceSegmentEventMapRepository : RepositoryBase<AudienceSegmentEventMap, int>, IAudienceSegmentEventMapRepository
    {
        public AudienceSegmentEventMapRepository(RepositoryImplBase<AudienceSegmentEventMap, int> repository)
            : base(repository)
        {
        }
    }
}
