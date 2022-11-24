using Noqoush.AdFalcon.Domain.Model.AppSite;
using Noqoush.AdFalcon.Domain.Repositories.Campaign;
using Noqoush.Framework.Persistence;


namespace Noqoush.AdFalcon.Persistence.Repositories.Campaign
{


    public class VideoMediaFileRepository : RepositoryBase<Domain.Model.Campaign.VideoMediaFile, int>, IVideoMediaFileRepository
    {
        public VideoMediaFileRepository(RepositoryImplBase<Domain.Model.Campaign.VideoMediaFile, int> repository)
            : base(repository)
        {
        }
    }
}
