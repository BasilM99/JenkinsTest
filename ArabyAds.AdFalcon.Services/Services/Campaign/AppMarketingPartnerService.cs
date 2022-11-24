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
namespace ArabyAds.AdFalcon.Services.Services.Campaign
{
    public class AppMarketingPartnerService : IAppMarketingPartnerService
    {
        private readonly IAppMarketingPartnerRepository appMarketingPartnerRep = null;
        public AppMarketingPartnerService(IAppMarketingPartnerRepository appMarketingPartnerRep)
        {
            this.appMarketingPartnerRep = appMarketingPartnerRep;
        }
        public IEnumerable<AppMarketingPartnerDto> GetAll()
        {
            var list = appMarketingPartnerRep.GetAll();
            return list.Select(t => MapperHelper.Map<AppMarketingPartnerDto>(t)).ToList();

        }
    }
}
