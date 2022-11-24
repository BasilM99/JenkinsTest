using Noqoush.AdFalcon.Domain.Model.Campaign;
using Noqoush.AdFalcon.Domain.Repositories.Campaign;
using Noqoush.Framework.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Noqoush.AdFalcon.Persistence.Repositories.Campaign
{
    public class AdGroupTrackingEventRepository : RepositoryBase<AdGroupTrackingEvent, int>, IAdGroupTrackingEventRepository
    {
        public AdGroupTrackingEventRepository(RepositoryImplBase<AdGroupTrackingEvent, int> repository)
            : base(repository)
        {
        }
    }



   public class AdGroupConversionEventRepository : RepositoryBase<AdGroupConversionEvent, int>, IAdGroupConversionEventRepository
    {
        public AdGroupConversionEventRepository(RepositoryImplBase<AdGroupConversionEvent, int> repository)
            : base(repository)
        {
        }
    }


    public class PixelEventMapRepository : RepositoryBase<PixelEventMap, int>, IPixelEventMapRepository
    {
        public PixelEventMapRepository(RepositoryImplBase<PixelEventMap, int> repository)
            : base(repository)
        {
        }
    }
}
