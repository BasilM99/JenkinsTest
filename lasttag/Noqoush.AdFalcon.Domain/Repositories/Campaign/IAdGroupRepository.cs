using Noqoush.Framework.Persistence;

namespace Noqoush.AdFalcon.Domain.Repositories.Campaign
{
    public interface IAdGroupRepository : IKeyedRepository<Model.Campaign.AdGroup, int>
    {
        string GetObjectName(int Id);
    }
}
