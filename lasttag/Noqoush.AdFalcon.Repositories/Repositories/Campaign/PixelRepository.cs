
using Noqoush.AdFalcon.Domain.Model.AppSite;
using Noqoush.AdFalcon.Domain.Model.Campaign;
using Noqoush.AdFalcon.Domain.Repositories.Campaign;
using Noqoush.Framework.Persistence;

namespace Noqoush.AdFalcon.Persistence.Repositories.Campaign
{
    

    public class PixelRepository : RepositoryBase<Pixel, int>, IPixelRepository
    {

        public PixelRepository(RepositoryImplBase<Pixel, int> repository)
          : base(repository)
        {
        }
    }
    public class AudienceSegmentPixelMapRepository : RepositoryBase<AudienceSegmentPixelMap, int>, IAudienceSegmentPixelMapRepository
    {

        public AudienceSegmentPixelMapRepository(RepositoryImplBase<AudienceSegmentPixelMap, int> repository)
          : base(repository)
        {
        }
    }

    
}
