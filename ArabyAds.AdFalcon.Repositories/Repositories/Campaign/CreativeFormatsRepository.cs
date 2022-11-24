
using ArabyAds.AdFalcon.Domain.Model.AppSite;
using ArabyAds.AdFalcon.Domain.Model.Campaign;
using ArabyAds.AdFalcon.Domain.Repositories.Campaign;
using ArabyAds.Framework.Persistence;

namespace ArabyAds.AdFalcon.Persistence.Repositories.Campaign
{
    public class CreativeFormatsRepository : RepositoryBase<CreativeFormat, int>, ICreativeFormatsRepository
    {   public CreativeFormatsRepository(RepositoryImplBase<CreativeFormat, int> repository)
            : base(repository)
        {
    }

}
    
}
