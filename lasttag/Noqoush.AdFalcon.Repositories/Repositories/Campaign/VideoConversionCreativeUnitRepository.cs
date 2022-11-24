using Noqoush.AdFalcon.Domain.Model.AppSite;
using Noqoush.AdFalcon.Domain.Repositories.Campaign;
using Noqoush.Framework.Persistence;


namespace Noqoush.AdFalcon.Persistence.Repositories.Campaign
{


    public class VideoConversionCreativeUnitRepository : RepositoryBase<Domain.Model.Campaign.VideoConversionCreativeUnit, int>, IVideoConversionCreativeUnitRepository
    {
        public VideoConversionCreativeUnitRepository(RepositoryImplBase<Domain.Model.Campaign.VideoConversionCreativeUnit, int> repository)
            : base(repository)
        {
        }
    }
}
