using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Noqoush.AdFalcon.Services.Interfaces.Services.Core;
using Noqoush.AdFalcon.Domain.Repositories.Core;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Core;
using Noqoush.AdFalcon.Services.Mapping;
using System.Linq.Expressions;
using Noqoush.AdFalcon.Domain.Model.Core;
using Noqoush.AdFalcon.Domain.Common.Model.Core;

namespace Noqoush.AdFalcon.Services.Services.Core
{
    public class DeviceService : IDeviceService
    {

        private IDeviceRepository deviceRepository = null;
        public DeviceService(IDeviceRepository deviceRepository)
        {
            this.deviceRepository = deviceRepository;
        }

        public IEnumerable<DeviceDto> GetAll()
        {
            var list = deviceRepository.GetAll();
            return list.Select(operatorDto => MapperHelper.Map<DeviceDto>(operatorDto)).ToList();
        }
        /// <summary>
        /// use this service operation to get Tree List of Device Objects  depend on the query
        /// </summary>
        /// <param name="criteria">query Text to search By</param>
        /// <param name="deviceTypeId">Filter the result base on this devicetypeid</param>
        /// <returns>Tree List of DeviceDto that match the query</returns>
        public IEnumerable<TreeDto> SearchByQueryTree(int deviceTypeId, string query)
        {
            IList<TreeDto> returnList = new List<TreeDto>();
            var list = deviceRepository.Query(p => deviceTypeId == (int)DeviceTypeEnum.Any || p.DeviceType.ID == deviceTypeId);

            var devices = list.Where(item => DeviceMatch(item, query));
            return BuildTreeWithExtra(devices);
        }

        public IEnumerable<TreeDto> GetAllDeviceTree()
        {
            var devices = deviceRepository.GetAll();
            return BuildTree(devices);
        }

        public IEnumerable<TreeDto> GetDeviceTree(int platformId, int deviceConstraint)
        {
            IEnumerable<Device> devices = null;
            if (deviceConstraint > 0)
            {
                devices = deviceRepository.Query(item => item.Platform.ID == platformId && item.DeviceType.ID == deviceConstraint);
            }
            else
            {
                devices = deviceRepository.Query(item => item.Platform.ID == platformId);
            }
            return BuildTree(devices);
        }

        private static IList<TreeDto> BuildTreeWithExtra(IEnumerable<Device> devices)
        {
            IList<TreeDto> returnList = new List<TreeDto>();
            var listByPlatform = (from t in devices
                                  group t by new { t.Platform }
                                      into grp
                                      select new
                                      {
                                          grp.Key.Platform,
                                          Manufacturers = grp.ToList()
                                      }).ToList();


            foreach (var paltform in listByPlatform)
            {
                var paltformTreeDto = new TreeDto
                {
                    Id = paltform.Platform.ID.ToString(),
                    Name = MapperHelper.Map<LocalizedStringDto>(paltform.Platform.Name),
                    Childs = new List<TreeDto>(),
                    Key = "Platforms"
                };

                var list = (from t in paltform.Manufacturers
                            group t by new { t.Manufacturer }
                                into grp
                                select new
                                {
                                    grp.Key.Manufacturer,
                                    Devices = grp.ToList()
                                }).ToList();

                foreach (var item in list)
                {
                    var manufacturerTreeDto = new TreeDto
                    {
                        Id = item.Manufacturer.ID.ToString(),
                        Name =
                            MapperHelper.Map<LocalizedStringDto>(item.Manufacturer.Name),
                        Childs = new List<TreeDto>(),
                        Key = "Manufacturers"
                    };

                    foreach (var device in item.Devices)
                    {
                        var treeDto = new TreeDto
                        {
                            Id = string.Format("{0}#{1}#{2}", device.ID, item.Manufacturer.ID.ToString(), paltform.Platform.ID.ToString()),
                            Name = MapperHelper.Map<LocalizedStringDto>(device.Name),
                            Childs = new List<TreeDto>(),
                            Key = "Models"
                        };
                        manufacturerTreeDto.Childs.Add(treeDto);
                    }
                    paltformTreeDto.Childs.Add(manufacturerTreeDto);
                }

                returnList.Add(paltformTreeDto);
            }
           
            return returnList;
        }
        private static IList<TreeDto> BuildTree(IEnumerable<Device> devices)
        {
            IList<TreeDto> returnList = new List<TreeDto>();
            var listByPlatform = (from t in devices
                                  group t by new { t.Platform }
                                      into grp
                                      select new
                                                 {
                                                     grp.Key.Platform,
                                                     Manufacturers = grp.ToList()
                                                 }).ToList();


            foreach (var paltform in listByPlatform)
            {
                var paltformTreeDto = new TreeDto
                                          {
                                              Id = paltform.Platform.ID.ToString(),
                                              Name = MapperHelper.Map<LocalizedStringDto>(paltform.Platform.Name),
                                              Childs = new List<TreeDto>(),
                                              Key = "Platforms"
                                          };

                var list = (from t in paltform.Manufacturers
                            group t by new { t.Manufacturer }
                                into grp
                                select new
                                           {
                                               grp.Key.Manufacturer,
                                               Devices = grp.ToList()
                                           }).ToList();

                foreach (var item in list)
                {
                    var manufacturerTreeDto = new TreeDto
                                                  {
                                                      Id = item.Manufacturer.ID.ToString(),
                                                      Name =
                                                          MapperHelper.Map<LocalizedStringDto>(item.Manufacturer.Name),
                                                      Childs = new List<TreeDto>(),
                                                      Key = "Manufacturers"
                                                  };

                    foreach (var device in item.Devices)
                    {
                        var treeDto = new TreeDto
                                          {
                                              Id = device.ID.ToString(),
                                              Name = MapperHelper.Map<LocalizedStringDto>(device.Name),
                                              Childs = new List<TreeDto>(),
                                              Key = "Models"
                                          };
                        manufacturerTreeDto.Childs.Add(treeDto);
                    }
                    paltformTreeDto.Childs.Add(manufacturerTreeDto);
                }

                returnList.Add(paltformTreeDto);
            }

            return returnList;
        }

        private bool DeviceMatch(Device deviceDto, string query)
        {
            var match = false;
            var words = query.ToLower().Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            match = words.Length == 0;

            foreach (var word in words)
            {
                if (deviceDto.Name.ToString().ToLower().Contains(word))
                {
                    match = true;
                    break;
                }
                if (deviceDto.Platform.Name.ToString().ToLower().Contains(word))
                {
                    match = true;
                    break;
                }
                if (deviceDto.Manufacturer.Name.ToString().ToLower().Contains(word))
                {
                    match = true;
                    break;
                }
            }
            return match;
        }
        public IEnumerable<DeviceDto> SearchByQuery(string query)
        {
            var list = deviceRepository.GetAll();
            return list.Where(item => DeviceMatch(item, query)).Select(operatorDto => MapperHelper.Map<DeviceDto>(operatorDto)).ToList();
        }
    }
}
