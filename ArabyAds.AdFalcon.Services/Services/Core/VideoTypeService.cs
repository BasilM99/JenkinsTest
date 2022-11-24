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
    public class VideoTypeService : IVideoTypeService
    {

        private IVideoTypeRepository _VideoTypeRepository = null;
        public VideoTypeService(IVideoTypeRepository videoTypeRepository)
        {

            this._VideoTypeRepository = videoTypeRepository;
        }

        IEnumerable<VideoTypeDto> IVideoTypeService.GetAll()
        {
            return new List<VideoTypeDto>();
        }
    }
}
