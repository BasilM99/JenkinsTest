//using System;
//using System.Collections.Concurrent;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using AutoMapper;
//using Noqoush.AdFalcon.Domain.Model.Campaign;
//using Noqoush.AdFalcon.Domain.Model.Core;
//using Noqoush.AdFalcon.Domain.Repositories;
//using Noqoush.AdFalcon.Domain.Repositories.Campaign;
//using Noqoush.AdFalcon.Services.Interfaces.DTOs.Campaign;
//using Noqoush.AdFalcon.Services.Interfaces.DTOs.Campaign.Targeting;
//using Noqoush.AdFalcon.Services.Interfaces.Services;
//using Noqoush.AdFalcon.Services.Interfaces.Services.Campaign;
//using Noqoush.AdFalcon.Services.Mapping;
//using Noqoush.Framework.DomainServices.Localization.Repositories;
//using Noqoush.Framework.Persistence;
//using Noqoush.AdFalcon.Common.UserInfo;
//using Noqoush.Framework;
//using Noqoush.AdFalcon.Domain.Repositories.Core;
//using Noqoush.AdFalcon.Domain.Model.Campaign.Targeting;
//using Noqoush.AdFalcon.Services.Interfaces.DTOs.Core;
//namespace Noqoush.AdFalcon.Services
//{
//    public class AdGroupStatusService : IAdGroupStatusService
//    {
//        private readonly IAdGroupStatusRepository adGroupStatusRepository = null;
//        public AdGroupStatusService(IAdGroupStatusRepository adGroupStatusRepository)
//        {
//            this.adGroupStatusRepository = adGroupStatusRepository;
//        }
//        //public IEnumerable<LookupDto> GetAll()
//        //{
//        //    var list = adGroupStatusRepository.GetAll();
//        //    return list.Select(appSiteTypeDto => MapperHelper.Map<LookupDto>(appSiteTypeDto)).ToList();

//        //}
//    }
//}
