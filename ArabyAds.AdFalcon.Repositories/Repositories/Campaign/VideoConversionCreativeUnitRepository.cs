using ArabyAds.AdFalcon.Domain.Model.AppSite;
using ArabyAds.AdFalcon.Domain.Repositories.Campaign;
using ArabyAds.Framework.Persistence;


namespace ArabyAds.AdFalcon.Persistence.Repositories.Campaign
{


    public class VideoConversionCreativeUnitRepository : RepositoryBase<Domain.Model.Campaign.VideoConversionCreativeUnit, int>, IVideoConversionCreativeUnitRepository
    {
        public VideoConversionCreativeUnitRepository(RepositoryImplBase<Domain.Model.Campaign.VideoConversionCreativeUnit, int> repository)
            : base(repository)
        {
        }
    }
}
