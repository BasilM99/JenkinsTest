using Noqoush.AdFalcon.Domain.Repositories.Campaign.Creative;
using Noqoush.Framework.Persistence;

namespace Noqoush.AdFalcon.Persistence.Repositories.Campaign.Creative
{
    public class TileImageSizeRepository : RepositoryBase<Domain.Model.Campaign.TileImageSize, int>, ITileImageSizeRepository
    {
        public TileImageSizeRepository(RepositoryImplBase<Domain.Model.Campaign.TileImageSize, int> repository)
            : base(repository)
        {
        }
    }
}
