using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArabyAds.AdFalcon.Services.Interfaces.Services.Core;
using ArabyAds.AdFalcon.Domain.Repositories.Core;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Core;
using ArabyAds.AdFalcon.Services.Mapping;
using System.Linq.Expressions;
using ArabyAds.AdFalcon.Domain.Model.Core;
using ArabyAds.AdFalcon.Domain.Common.Model.Core;
using ArabyAds.AdFalcon.Services.Interfaces.Messages;

namespace ArabyAds.AdFalcon.Services.Services.Core
{
    public class DeviceService : IDeviceService
    {

        private IDeviceRepository deviceRepository = null;
        private IDeviceCodeRepository deviceCodeRepository = null;

        private IManufacturerRepository manufacturerRepository = null;
        public DeviceService(IDeviceRepository deviceRepository, IManufacturerRepository ManufacturerRepository, IDeviceCodeRepository DeviceCodeRepository)
        {
            this.deviceRepository = deviceRepository;

            this.manufacturerRepository = ManufacturerRepository;
            this.deviceCodeRepository = DeviceCodeRepository;
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
        public IEnumerable<TreeDto> SearchByQueryTree(SearchByQueryTreeRequest request)
        {
            IList<TreeDto> returnList = new List<TreeDto>();
            var list = deviceRepository.Query(p => request.DeviceTypeId == (int)DeviceTypeEnum.Any || p.DeviceType.ID == request.DeviceTypeId);

            var devices = list.Where(item => DeviceMatch(item, request.Query));
            return BuildTreeWithExtra(devices);
        }

        public IEnumerable<TreeDto> GetAllDeviceTree()
        {
            var devices = deviceRepository.GetAll();
            return BuildTree(devices);
        }

        public IEnumerable<TreeDto> GetDeviceTree(GetDeviceTreeRequest request)
        {
            IEnumerable<Device> devices = null;
            if (request.DeviceConstraint > 0)
            {
                devices = deviceRepository.Query(item => item.Platform.ID == request.PlatformId && item.DeviceType.ID == request.DeviceConstraint);
            }
            else
            {
                devices = deviceRepository.Query(item => item.Platform.ID == request.PlatformId);
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

        public IEnumerable<DeviceDto> SearchByQueryandDeviceType(SearchByQueryTreeRequest request)
        {
            var list = deviceRepository.Query(p => request.DeviceTypeId == (int)DeviceTypeEnum.Any || p.DeviceType.ID == request.DeviceTypeId);
               var manufacturers = manufacturerRepository.GetAll();
            var codes = deviceCodeRepository.GetAll();
            var match = false;
            var matchCode = false;

            var words = request.Query.ToLower().Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            foreach (var word in words)
            {
                if (manufacturers.Where(M=>M.Name.Value.ToString().ToLower().Contains(word)).FirstOrDefault()!=null  )
                {
                    match = true;
                    break;
                }

            }

            foreach (var word in words)
            {
                if (codes.Where(M => M.Code.ToString().ToLower().Contains(word)).FirstOrDefault() != null)
                {
                    matchCode = true;
                    break;
                }

            }

            return list.Where(item => DeviceMatchLess(item, request.Query, match, matchCode)).Select(operatorDto => MapperHelper.Map<DeviceDto>(operatorDto)).ToList();
        }


        private bool DeviceMatchLess(Device deviceDto, string query, bool searchManufacture = false, bool searchDeviceCode=false)
        {
            var match = false;
            var words = query.ToLower().Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            match = words.Length == 0;
            if (searchManufacture)
            {
                foreach (var word in words)
                {
                    if (deviceDto.Name.ToString().ToLower().Contains(word))
                    {
                        match = true;
                        break;
                    }
                    /* if (deviceDto.Platform.Name.ToString().ToLower().Contains(word))
                     {
                         match = true;
                         break;
                     }*/
                    /*if (deviceDto.Manufacturer.Name.ToString().ToLower().Contains(word))
                    {
                        match = true;
                        break;
                    }*/

                    if (searchDeviceCode)
                    {
                     
                        if (deviceDto.Codes.Where(M=>M.Code.ToString().ToLower().Contains(word) && word.Length>2 ).FirstOrDefault()!=null)
                        {
                            match = true;
                            break;
                        }
                    }
                }
                if (match)
                {
                    match = false;
                    foreach (var word in words)
                    {
                        if (deviceDto.Manufacturer.Name.ToString().ToLower().Contains(word))
                        {
                            match = true;
                            break;
                        }

                    }
                }

            }
            else
            {

                foreach (var word in words)
                {
                    if (deviceDto.Name.ToString().ToLower().Contains(word))
                    {
                        match = true;
                        break;
                    }
                    /* if (deviceDto.Platform.Name.ToString().ToLower().Contains(word))
                     {
                         match = true;
                         break;
                     }*/
                    if (deviceDto.Manufacturer.Name.ToString().ToLower().Contains(word))
                    {
                        match = true;
                        break;
                    }

                    if (searchDeviceCode)
                    {
                    
                        if (deviceDto.Codes.Where(M => M.Code.ToString().ToLower().Contains(word) && word.Length > 2 ).FirstOrDefault() != null)
                        {
                            match = true;
                            break;
                        }
                    }
                }
               
            }
            
            return match;
        }


    }
}
