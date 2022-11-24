using Noqoush.AdFalcon.Domain.Model.AppSite;
using Noqoush.AdFalcon.Domain.Model.Campaign;
using Noqoush.AdFalcon.Domain.Repositories.Campaign;
using Noqoush.Framework.Persistence;

namespace Noqoush.AdFalcon.Persistence.Repositories.Campaign
{
    public class VideoTypeRepository : RepositoryBase<VideoType, int>, IVideoTypeRepository
    {
        public VideoTypeRepository(RepositoryImplBase<VideoType, int> repository)
            : base(repository)
        {
        }
    }
}
