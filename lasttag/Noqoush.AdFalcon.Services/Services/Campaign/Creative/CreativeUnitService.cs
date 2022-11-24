using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Noqoush.AdFalcon.Domain.Repositories.Campaign;
using Noqoush.AdFalcon.Domain.Repositories.Campaign.Creative;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Campaign.Creative;
using Noqoush.AdFalcon.Services.Interfaces.Services.Campaign.Creative;
using Noqoush.AdFalcon.Services.Mapping;
using Noqoush.AdFalcon.Domain.Model.Core;
using Noqoush.AdFalcon.Domain.Model.Campaign;
using Noqoush.AdFalcon.Domain.Common.Model.Core;
using Noqoush.AdFalcon.Domain.Common.Model.Campaign;

namespace Noqoush.AdFalcon.Services.Services.Campaign.Creative
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

        public IEnumerable<CreativeUnitDto> GetBy(DeviceTypeEnum deviceType, AdTypeIds? adType, AdSubTypes? adSubType, string group)
        {
            var result = new List<CreativeUnitDto>();
            var creativeUnits = _adSupportedCreativeUnitRepository.GetAll().Where(x => (deviceType == (int)DeviceTypeEnum.Any || x.CreativeUnit.DeviceType.ID == (int)deviceType) &&
                      (!adType.HasValue || x.AdType == null || x.AdType.ID == (int)adType.Value) &&
                      (!adSubType.HasValue || !x.AdSubType.HasValue || x.AdSubType.Value == adSubType.Value) &&
                      (string.IsNullOrEmpty(group) || x.CreativeUnit.Groups.Any(p => p.Code == group)))
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

        public IEnumerable<CreativeUnitDto> GetByDeviceType(int deviceType)
        {
            var list = _creativeUnitRepository.Query(item => item.DeviceType.ID == deviceType).OrderByDescending(item => item.Width);
            return list.Select(creativeUnitDto => MapperHelper.Map<CreativeUnitDto>(creativeUnitDto)).ToList();
        }


        public CreativeUnitDto GetById(int id)
        {
            var creativeUnit = _creativeUnitRepository.Get(id);
            return creativeUnit != null ? MapperHelper.Map<CreativeUnitDto>(creativeUnit) : null;
        }

        public List<CreativeUnitDto> GetByCriteria(int? creativeUnitId, int deviceTypeId, string group, int? adTypeId)
        {
            var list = _creativeUnitRepository.Query(item => (!creativeUnitId.HasValue || item.ID == creativeUnitId) &&
                                                             (deviceTypeId == (int)DeviceTypeEnum.Any || item.DeviceType.ID == deviceTypeId) &&
                                                             (string.IsNullOrEmpty(group) || item.Groups.Any(x => x.Code == group)) &&
                                                             (!adTypeId.HasValue || item.SupportedTypes.Any(z => z.AdType.ID == adTypeId)))
                                                             .OrderByDescending(item => item.Width);

            return list != null ? list.Select(p => MapperHelper.Map<CreativeUnitDto>(p)).ToList() : null;
        }

        public List<CreativeUnitDto> GetByCriteriaWidthHeight(int? creativeUnitId, int deviceTypeId, string group, int? adTypeId, int Width, int Height)
        {
            var list = _creativeUnitRepository.Query(item => (!creativeUnitId.HasValue || item.ID == creativeUnitId) &&
                                                             (deviceTypeId == (int)DeviceTypeEnum.Any || item.DeviceType.ID == deviceTypeId) &&
                                                             (string.IsNullOrEmpty(group) || item.Groups.Any(x => x.Code == group)) && (item.Width== Width) && (item.Height == Height) &&
                                                             (!adTypeId.HasValue || item.SupportedTypes.Any(z => z.AdType.ID == adTypeId)))
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


        public string GetGroupByCreativeByID(int CreativeId)
        {
            var ItemCreative = _creativeUnitRepository.Query(item =>
                                                            item.ID == CreativeId
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
