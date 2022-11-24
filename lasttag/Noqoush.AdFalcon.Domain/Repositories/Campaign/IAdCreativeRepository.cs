using Noqoush.Framework.Persistence;

namespace Noqoush.AdFalcon.Domain.Repositories.Campaign
{
    public interface IAdCreativeRepository : IKeyedRepository<Model.Campaign.AdCreative, int>
    {
        string GetObjectName(int Id);
    }

    public interface IAdExtensionRepository : IKeyedRepository<Model.Campaign.AdExtension, int>
    {
        
    }
}
