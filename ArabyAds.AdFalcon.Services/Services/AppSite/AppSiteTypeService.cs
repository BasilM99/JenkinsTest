using System.Collections.Generic;
using System.Linq;
using ArabyAds.AdFalcon.Domain.Repositories;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.AppSite;
using ArabyAds.AdFalcon.Services.Interfaces.Services.AppSite;
using ArabyAds.AdFalcon.Services.Mapping;
using ArabyAds.Framework;

namespace ArabyAds.AdFalcon.Services.Services.AppSite
{

    public class AppSiteTypeService : IAppSiteTypeService
    {
        private IAppSiteTypeRepository appSiteTypeRepository = null;
        public AppSiteTypeService(IAppSiteTypeRepository appSiteTypeRepository)
        {
            this.appSiteTypeRepository = appSiteTypeRepository;
        }


        public IEnumerable<AppSiteTypeDto> GetAll()
        {
            var list = appSiteTypeRepository.GetAll();
            return list.Select(appSiteTypeDto => MapperHelper.Map<AppSiteTypeDto>(appSiteTypeDto)).ToList();
        }

        public AppSiteTypeDto Get(ValueMessageWrapper<int> id)
        {
            var item = appSiteTypeRepository.Get(id.Value);
            if (item != null)
            {
                return MapperHelper.Map<AppSiteTypeDto>(item);
            }
            else
            {
                return null;
            }
        }
    }
}
