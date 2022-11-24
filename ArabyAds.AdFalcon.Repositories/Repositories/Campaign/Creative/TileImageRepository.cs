using ArabyAds.AdFalcon.Domain.Repositories.Campaign.Creative;
using ArabyAds.Framework.Persistence;

namespace ArabyAds.AdFalcon.Persistence.Repositories.Campaign.Creative
{
    public class TileImageRepository : RepositoryBase<Domain.Model.Campaign.TileImage, int>, ITileImageRepository
    {
        public TileImageRepository(RepositoryImplBase<Domain.Model.Campaign.TileImage, int> repository)
            : base(repository)
        {
        }
    }
}
