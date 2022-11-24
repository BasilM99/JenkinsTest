using Noqoush.AdFalcon.Domain.Model.Campaign;
using Noqoush.AdFalcon.Domain.Repositories.Campaign;
using Noqoush.AdFalcon.Domain.Repositories.Core;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Core;
using Noqoush.AdFalcon.Services.Interfaces.Services.Core;
using Noqoush.AdFalcon.Services.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Noqoush.AdFalcon.Services.Services.Core
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
