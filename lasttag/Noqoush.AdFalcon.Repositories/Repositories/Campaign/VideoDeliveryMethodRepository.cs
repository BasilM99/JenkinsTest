using Noqoush.AdFalcon.Domain.Model.AppSite;
using Noqoush.AdFalcon.Domain.Model.Campaign;
using Noqoush.AdFalcon.Domain.Repositories.Campaign;
using Noqoush.Framework.Persistence;

namespace Noqoush.AdFalcon.Persistence.Repositories.Campaign
{
    public class VideoDeliveryMethodRepository : RepositoryBase<VideoDeliveryMethod, int>, IVideoDeliveryMethodRepository
    {
        public VideoDeliveryMethodRepository(RepositoryImplBase<VideoDeliveryMethod, int> repository)
            : base(repository)
        {
        }
    }
}
