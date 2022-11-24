using ArabyAds.AdFalcon.Server.Integration.Services.Model;
using System.Threading.Tasks;

namespace ArabyAds.AdFalcon.Server.Integration.Services
{
    public interface IUserOptService
    {
        Task<bool> IsTrackingEnabled(string userId);
        Task<string> UpdateTracking(UpdateTrackingRequest request);
    }
}