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
