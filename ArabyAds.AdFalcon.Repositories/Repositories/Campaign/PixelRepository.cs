
using ArabyAds.AdFalcon.Domain.Model.AppSite;
using ArabyAds.AdFalcon.Domain.Model.Campaign;
using ArabyAds.AdFalcon.Domain.Repositories.Campaign;
using ArabyAds.Framework.Persistence;

namespace ArabyAds.AdFalcon.Persistence.Repositories.Campaign
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
