using ArabyAds.AdFalcon.Domain.Model.AppSite;
using ArabyAds.AdFalcon.Domain.Model.Campaign;
using ArabyAds.AdFalcon.Domain.Repositories.Campaign;
using ArabyAds.Framework.Persistence;

namespace ArabyAds.AdFalcon.Persistence.Repositories.Campaign
{
    public class VideoTypeRepository : RepositoryBase<VideoType, int>, IVideoTypeRepository
    {
        public VideoTypeRepository(RepositoryImplBase<VideoType, int> repository)
            : base(repository)
        {
        }
    }
}
