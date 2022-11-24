using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using ArabyAds.AdFalcon.Domain.Model.Campaign;
using ArabyAds.AdFalcon.Domain.Model.Core;
using ArabyAds.AdFalcon.Domain.Repositories;
using ArabyAds.AdFalcon.Domain.Repositories.Campaign;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Campaign;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Campaign.Objective;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Campaign.Targeting;
using ArabyAds.AdFalcon.Services.Interfaces.Services;
using ArabyAds.AdFalcon.Services.Interfaces.Services.Campaign;
using ArabyAds.AdFalcon.Services.Mapping;
using ArabyAds.Framework.DomainServices.Localization.Repositories;
using ArabyAds.Framework.Persistence;
using ArabyAds.AdFalcon.Common.UserInfo;
using ArabyAds.Framework;
using ArabyAds.AdFalcon.Domain.Repositories.Core;
using ArabyAds.AdFalcon.Domain.Model.Campaign.Targeting;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Core;
namespace ArabyAds.AdFalcon.Services
{
    public class ObjectiveTypeService : IObjectiveTypeService
    {
        private readonly IObjectiveTypeRepository objectiveTypeRepository = null;
        private readonly IAdActionTypeRepository adActionTypeRepository = null;
        public ObjectiveTypeService(IObjectiveTypeRepository objectiveTypeRepository ,IAdActionTypeRepository adActionTypeRepository)
        {
            this.objectiveTypeRepository = objectiveTypeRepository;
            this.adActionTypeRepository = adActionTypeRepository;
        }
        public IEnumerable<ObjectiveTypeDto> GetAll()
        {
            var list = objectiveTypeRepository.GetAll();
            return list.Select(appSiteTypeDto => MapperHelper.Map<ObjectiveTypeDto>(appSiteTypeDto)).ToList();

        }

        public IEnumerable<AdActionTypeDto> GetAdActionTypeAllForWeb()
        {
            var list = adActionTypeRepository.GetAll().Where(M=>M.ShowForObjective == true);

            
            return list.Select(appSiteTypeDto => MapperHelper.Map<AdActionTypeDto>(appSiteTypeDto)).ToList();

        }
    }
}
