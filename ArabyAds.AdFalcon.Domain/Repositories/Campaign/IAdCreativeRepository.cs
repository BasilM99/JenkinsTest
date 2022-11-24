using ArabyAds.Framework.Persistence;

namespace ArabyAds.AdFalcon.Domain.Repositories.Campaign
{
    public interface IAdCreativeRepository : IKeyedRepository<Model.Campaign.AdCreative, int>
    {
        string GetObjectName(int Id);
    }

    public interface IAdExtensionRepository : IKeyedRepository<Model.Campaign.AdExtension, int>
    {
        
    }
}
