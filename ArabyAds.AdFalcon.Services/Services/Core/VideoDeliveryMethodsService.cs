using ArabyAds.AdFalcon.Domain.Model.Campaign;
using ArabyAds.AdFalcon.Domain.Repositories.Campaign;
using ArabyAds.AdFalcon.Domain.Repositories.Core;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Core;
using ArabyAds.AdFalcon.Services.Interfaces.Services.Core;
using ArabyAds.AdFalcon.Services.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArabyAds.AdFalcon.Services.Services.Core
{
    public class VideoDeliveryMethodsService : IVideoDeliveryMethodsService
    {

        private IVideoDeliveryMethodRepository _VideoDeliveryMethodRepository = null;
        public VideoDeliveryMethodsService(IVideoDeliveryMethodRepository videoDeliveryMethodRepository)
        {

            this._VideoDeliveryMethodRepository = videoDeliveryMethodRepository;
        }

        IEnumerable<VideoDeliveryMethodDto> IVideoDeliveryMethodsService.GetAll()
        {
            var list = _VideoDeliveryMethodRepository.GetAll();
            return list.Select(videoDeliveryMethods => MapperHelper.Map<VideoDeliveryMethodDto>(videoDeliveryMethods)).ToList();
        }
    }
}
