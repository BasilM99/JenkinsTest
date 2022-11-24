using ArabyAds.AdFalcon.Domain.Model.AppSite;
using ArabyAds.AdFalcon.Domain.Repositories.Campaign;
using ArabyAds.Framework.Persistence;


namespace ArabyAds.AdFalcon.Persistence.Repositories.Campaign
{


    public class VideoMediaFileRepository : RepositoryBase<Domain.Model.Campaign.VideoMediaFile, int>, IVideoMediaFileRepository
    {
        public VideoMediaFileRepository(RepositoryImplBase<Domain.Model.Campaign.VideoMediaFile, int> repository)
            : base(repository)
        {
        }
    }
}
