using ArabyAds.AdFalcon.Server.Integration.Services.Model;
using System.Threading.Tasks;

namespace ArabyAds.AdFalcon.Server.Integration.Services
{
    public interface IAudienceListService
    {
        Task UpdateAudienceList(UpdateAudienceListRequest request);
    }
}
