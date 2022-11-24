using ArabyAds.AdFalcon.Domain.Repositories.Campaign;
using ArabyAds.AdFalcon.Domain.Repositories.Core;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Campaign;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Core;
using ArabyAds.AdFalcon.Services.Interfaces.Services.Campaign;
using ArabyAds.AdFalcon.Services.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArabyAds.AdFalcon.Services.Services.Campaign
{
    public class TrackingEventService : ITrackingEventService
    {
        private ITrackingEventRepository _TrackingEventRepository;
        private ICostModelWrapperRepository _CostModelWrapperRepository;

        public TrackingEventService(ITrackingEventRepository trackingEventRepository, ICostModelWrapperRepository costModelWrapperRepository)
        {
            _TrackingEventRepository = trackingEventRepository;
            _CostModelWrapperRepository = costModelWrapperRepository;
        }

        public IEnumerable<TrackingEventDto> GetCostModelEvents()
        {
            var costModelWrappers = _CostModelWrapperRepository.GetAll();

            var costModelTrackingEvents = costModelWrappers.Where(M=>M.Event!=null).Select(p => p.Event);

            if (costModelTrackingEvents != null)
            {
                var trackingEventDtoList = costModelTrackingEvents.Select(p => MapperHelper.Map<TrackingEventDto>(p)).ToList();
                return trackingEventDtoList;
            }

            return null;
        }
    }
}
