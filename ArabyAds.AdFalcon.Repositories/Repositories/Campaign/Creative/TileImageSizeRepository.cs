using ArabyAds.AdFalcon.Domain.Repositories.Campaign.Creative;
using ArabyAds.Framework.Persistence;

namespace ArabyAds.AdFalcon.Persistence.Repositories.Campaign.Creative
{
    public class TileImageSizeRepository : RepositoryBase<Domain.Model.Campaign.TileImageSize, int>, ITileImageSizeRepository
    {
        public TileImageSizeRepository(RepositoryImplBase<Domain.Model.Campaign.TileImageSize, int> repository)
            : base(repository)
        {
        }
    }
}
