using ArabyAds.AdFalcon.Domain.Model.Campaign.Targeting;
using ArabyAds.AdFalcon.Domain.Repositories.Campaign;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArabyAds.AdFalcon.Domain.Services
{
    public class Targeting : ITargeting
    {
        private ITargetingBaseRepository _TargetingRepository;


        public IList<AdRequestTargeting> GetAdRequests(int AdGroupId)
        {
            var AdRequests = _TargetingRepository.Query(x => x.AdGroup.ID == AdGroupId).Select(x => x.AdGroup.Targetings).OfType<AdRequestTargeting>().ToList();

            return AdRequests;

        }


    }
}
