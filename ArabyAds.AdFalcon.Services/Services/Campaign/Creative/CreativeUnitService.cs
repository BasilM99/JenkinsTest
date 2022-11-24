using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using ArabyAds.AdFalcon.Domain.Repositories.Campaign;
using ArabyAds.AdFalcon.Domain.Repositories.Campaign.Creative;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Campaign.Creative;
using ArabyAds.AdFalcon.Services.Interfaces.Services.Campaign.Creative;
using ArabyAds.AdFalcon.Services.Mapping;
using ArabyAds.AdFalcon.Domain.Model.Core;
using ArabyAds.AdFalcon.Domain.Model.Campaign;
using ArabyAds.AdFalcon.Domain.Common.Model.Core;
using ArabyAds.AdFalcon.Domain.Common.Model.Campaign;
using ArabyAds.AdFalcon.Services.Interfaces.Messages;
using ArabyAds.Framework;

namespace ArabyAds.AdFalcon.Services.Services.Campaign.Creative
{
    public class CreativeUnitService : ICreativeUnitService
    {
        private readonly ICreativeUnitRepository _creativeUnitRepository = null;
        private readonly IAdSupportedCreativeUnitRepository _adSupportedCreativeUnitRepository = null;
        public CreativeUnitService(ICreativeUnitRepository creativeUnitRepository, IAdSupportedCreativeUnitRepository adSupportedCreativeUnitRepository)
        {
            _creativeUnitRepository = creativeUnitRepository;
            _adSupportedCreativeUnitRepository = adSupportedCreativeUnitRepository;
        }
        public IEnumerable<CreativeUnitDto> GetAll()
        {
            var list = _creativeUnitRepository.GetAll();
            return list.Select(creativeUnitDto => MapperHelper.Map<CreativeUnitDto>(creativeUnitDto)).ToList();
        }

        public IEnumerable<CreativeUnitDto> GetBy(GetCreativeUnitRequest request)
        {
            var result = new List<CreativeUnitDto>();
            var creativeUnits = _adSupportedCreativeUnitRepository.Query(x => (request.DeviceType == (int)DeviceTypeEnum.Any || x.CreativeUnit.DeviceType.ID == (int)request.DeviceType) &&
                      (!request.AdType.HasValue || x.AdType == null || x.AdType.ID == (int)request.AdType) &&
                      (!request.AdSubType.HasValue || !x.AdSubType.HasValue || x.AdSubType.Value == request.AdSubType) &&
                      (string.IsNullOrEmpty(request.Group) || x.CreativeUnit.Groups.Any(p => p.Code == request.Group)))
                      .Select(x => new { x.CreativeUnit, x.EnvironmentType, x.RequiredType, x.OrientationReplacement, x.ID }).ToList();

            foreach (var creativeUnit in creativeUnits)
            {
                var creativeUnitDto = MapperHelper.Map<CreativeUnitDto>(creativeUnit.CreativeUnit);
                creativeUnitDto.EnvironmentType = creativeUnit.EnvironmentType;
                creativeUnitDto.RequiredType = (int)creativeUnit.RequiredType;
                creativeUnitDto.AdSupportedId = creativeUnit.ID;
                creativeUnitDto.OrientationReplacementId = creativeUnit.OrientationReplacement == null ? null : new int?(creativeUnit.OrientationReplacement.ID);
                if (creativeUnitDto.ID != 21) result.Add(creativeUnitDto);
            }
            return result;
        }
        public IEnumerable<CreativeUnitDto> GetAllBy()
        {
            var result = new List<CreativeUnitDto>();
            var creativeUnits = _adSupportedCreativeUnitRepository.GetAll()
                      .Select(x => new { x.CreativeUnit, x.EnvironmentType, x.RequiredType, x.OrientationReplacement, x.ID,x.AdType, x.AdSubType }).ToList();

            foreach (var creativeUnit in creativeUnits)
            {
                var creativeUnitDto = MapperHelper.Map<CreativeUnitDto>(creativeUnit.CreativeUnit);
                creativeUnitDto.EnvironmentType = creativeUnit.EnvironmentType;
                creativeUnitDto.RequiredType = (int)creativeUnit.RequiredType;
                creativeUnitDto.AdSupportedId = creativeUnit.ID;
                creativeUnitDto.AdType = creativeUnit.AdType!=null ? creativeUnit.AdType.ID: 0;
                creativeUnitDto.groupCodes = creativeUnit.CreativeUnit.Groups.Select(M => M.Code).ToList();
                creativeUnitDto.AdSubType = creativeUnit.AdSubType.HasValue? creativeUnit.AdSubType.Value:(AdSubTypes?)null;
                creativeUnitDto.OrientationReplacementId = creativeUnit.OrientationReplacement == null ? null : new int?(creativeUnit.OrientationReplacement.ID);
                if (creativeUnitDto.ID != 21) result.Add(creativeUnitDto);
            }
            return result;
        }



        public IEnumerable<CreativeUnitDto> GetByDeviceType(int deviceType)
        {
            var list = _creativeUnitRepository.Query(item => item.DeviceType.ID == deviceType).OrderByDescending(item => item.Width);
            return list.Select(creativeUnitDto => MapperHelper.Map<CreativeUnitDto>(creativeUnitDto)).ToList();
        }


        public CreativeUnitDto GetById(ValueMessageWrapper<int> id)
        {
            var creativeUnit = _creativeUnitRepository.Get(id.Value);
            return creativeUnit != null ? MapperHelper.Map<CreativeUnitDto>(creativeUnit) : null;
        }

        public List<CreativeUnitDto> GetByCriteria(GetCreativeUnitByCriteriaRequest request)
        {
            var list = _creativeUnitRepository.Query(item => (!request.CreativeUnitId.HasValue || item.ID == request.CreativeUnitId) &&
                                                             (request.DeviceTypeId == (int)DeviceTypeEnum.Any || item.DeviceType.ID == request.DeviceTypeId) &&
                                                             (string.IsNullOrEmpty(request.Group) || item.Groups.Any(x => x.Code == request.Group)) &&
                                                             (!request.AdTypeId.HasValue || item.SupportedTypes.Any(z => z.AdType.ID == request.AdTypeId)))
                                                             .OrderByDescending(item => item.Width);

            return list != null ? list.Select(p => MapperHelper.Map<CreativeUnitDto>(p)).ToList() : null;
        }

        public List<CreativeUnitDto> GetByCriteriaWithouDeviceType(GetCreativeUnitByCriteriaRequest request)
        {
            IList<CreativeUnit> list = null;
            if(!request.AdSubTypeId.HasValue)
               list = _creativeUnitRepository.Query(item => (!request.CreativeUnitId.HasValue || item.ID == request.CreativeUnitId) &&
                                                            // (request.DeviceTypeId == (int)DeviceTypeEnum.Any || item.DeviceType.ID == request.DeviceTypeId) &&
                                                             (string.IsNullOrEmpty(request.Group) || item.Groups.Any(x => x.Code == request.Group)) &&
                                                             (!request.AdTypeId.HasValue || item.SupportedTypes.Any(z => z.AdType.ID == request.AdTypeId)))
                                                             .OrderByDescending(item => item.Width).ToList();

            
            else
                list = _creativeUnitRepository.Query(item => (!request.CreativeUnitId.HasValue || item.ID == request.CreativeUnitId) &&
                                                             // (request.DeviceTypeId == (int)DeviceTypeEnum.Any || item.DeviceType.ID == request.DeviceTypeId) &&
                                                             (string.IsNullOrEmpty(request.Group) || item.Groups.Any(x => x.Code == request.Group)) &&
                                                             (!request.AdTypeId.HasValue || item.SupportedTypes.Any(z => z.AdType.ID == request.AdTypeId  && z.AdSubType== (AdSubTypes)request.AdSubTypeId.Value)))
                                                             .OrderByDescending(item => item.Width).ToList();



            return list != null ? list.Select(p => MapperHelper.Map<CreativeUnitDto>(p)).ToList() : null;
        }

        public List<CreativeUnitDto> GetByCriteriaWidthHeight(GetCreativeUnitByCriteriaWithDimensionsRequest request)
        {
            var list = _creativeUnitRepository.Query(item => (!request.CreativeUnitId.HasValue || item.ID == request.CreativeUnitId) &&
                                                             (request.DeviceTypeId == (int)DeviceTypeEnum.Any || item.DeviceType.ID == request.DeviceTypeId) &&
                                                             (string.IsNullOrEmpty(request.Group) || item.Groups.Any(x => x.Code == request.Group)) && (item.Width== request.Width) && (item.Height == request.Height) &&
                                                             (!request.AdTypeId.HasValue || item.SupportedTypes.Any(z => z.AdType.ID == request.AdTypeId)))
                                                             .OrderByDescending(item => item.Width);

            return list != null ? list.Select(p => MapperHelper.Map<CreativeUnitDto>(p)).ToList() : null;
        }
        public List<CreativeUnitDto> GetByGroupCode(string group)
        {
            var list = _creativeUnitRepository.Query(item => 
                                                             (string.IsNullOrEmpty(group) || item.Groups.Any(x => x.Code == group)) 
                                                           )
                                                             .OrderByDescending(item => item.Width);

            return list != null ? list.Select(p => MapperHelper.Map<CreativeUnitDto>(p)).ToList() : null;
        }


        public string GetGroupByCreativeCode(string CreativeCode)
        {
            var ItemCreative = _creativeUnitRepository.Query(item =>
                                                            item.Code== CreativeCode
                                                           )
                                                             .Single();

           var group = ItemCreative.Groups.Where(M => M.Code == "14" || M.Code == "15" || M.Code == "16" || M.Code == "17").SingleOrDefault();
            return group.Code;

        }


        public string GetGroupByCreativeByID(ValueMessageWrapper<int> CreativeId)
        {
            var ItemCreative = _creativeUnitRepository.Query(item =>
                                                            item.ID == CreativeId.Value
                                                           )
                                                             .Single();

            var group = ItemCreative.Groups.Where(M => M.Code == "14" || M.Code == "15" || M.Code == "16" || M.Code == "17").SingleOrDefault();
            return group.Code;

        }
        public IEnumerable<string> GetAllSupportedFormat()
        {
            var list = _creativeUnitRepository.GetAll();
            var formates = list.SelectMany(x => x.Formats).ToList();
            return formates.Select(x => x.Format).Distinct().ToList();
        }
    }
}
