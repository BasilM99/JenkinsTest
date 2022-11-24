using System.Collections.Generic;
using System.Linq;
using ArabyAds.AdFalcon.Domain.Repositories;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.AppSite;
using ArabyAds.AdFalcon.Services.Interfaces.Services.AppSite;
using ArabyAds.AdFalcon.Services.Mapping;

namespace ArabyAds.AdFalcon.Services.Services.AppSite
{

    public class AppSiteStatusService : IAppSiteStatusService
    {
        private readonly IAppSiteStatusRepository appSiteStatusRepository = null;
        public AppSiteStatusService(IAppSiteStatusRepository appSiteStatusRepository)
        {
            this.appSiteStatusRepository = appSiteStatusRepository;
        }


        public IEnumerable<AppSiteStatusDto> GetAll()
        {
            var list = appSiteStatusRepository.GetAll();
            return list.Select(appSiteStatusDto => MapperHelper.Map<AppSiteStatusDto>(appSiteStatusDto)).ToList();
        }

        //public AppSiteStatusDto Get(int id)
        //{
        //    var item = appSiteStatusRepository.Get(id);
        //    if (item != null)
        //    {
        //        return MapperHelper.Map<AppSiteStatusDto>(item);
        //    }
        //    else
        //    {
        //        return null;
        //    }
        //}
    }
}
