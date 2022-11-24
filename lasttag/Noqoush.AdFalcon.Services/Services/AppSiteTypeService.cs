using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using Noqoush.AdFalcon.Domain.Repositories;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.AppSite;
using Noqoush.AdFalcon.Services.Interfaces.Services;
using Noqoush.AdFalcon.Services.Interfaces.Services.AppSite;
using Noqoush.AdFalcon.Services.Mapping;
using Noqoush.Framework.Persistence;

namespace Noqoush.AdFalcon.Services
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

        public AppSiteTypeDto Get(int id)
        {
            var item = appSiteTypeRepository.Get(id);
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
