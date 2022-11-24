using Noqoush.AdFalcon.Domain.Repositories.Campaign.Creative;
using Noqoush.Framework.Persistence;

namespace Noqoush.AdFalcon.Persistence.Repositories.Campaign.Creative
{
    public class TileImageRepository : RepositoryBase<Domain.Model.Campaign.TileImage, int>, ITileImageRepository
    {
        public TileImageRepository(RepositoryImplBase<Domain.Model.Campaign.TileImage, int> repository)
            : base(repository)
        {
        }
    }
}
