using ArabyAds.AdFalcon.Domain.Model.AppSite;
using ArabyAds.AdFalcon.Domain.Model.Campaign;
using ArabyAds.AdFalcon.Domain.Repositories.Campaign;
using ArabyAds.Framework.Persistence;

namespace ArabyAds.AdFalcon.Persistence.Repositories.Campaign
{
    public class VideoDeliveryMethodRepository : RepositoryBase<VideoDeliveryMethod, int>, IVideoDeliveryMethodRepository
    {
        public VideoDeliveryMethodRepository(RepositoryImplBase<VideoDeliveryMethod, int> repository)
            : base(repository)
        {
        }
    }
}
