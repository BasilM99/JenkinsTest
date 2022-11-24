using Noqoush.AdFalcon.Domain.Model.Campaign;
using Noqoush.AdFalcon.Domain.Model.Core;
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
    public class TrackingEventRepository : RepositoryBase<TrackingEvent, int>, ITrackingEventRepository
    {
        public TrackingEventRepository(RepositoryImplBase<TrackingEvent, int> repository)
            : base(repository)
        {
        }
    }
}
