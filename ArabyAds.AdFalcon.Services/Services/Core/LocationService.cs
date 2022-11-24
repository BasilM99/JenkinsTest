using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArabyAds.AdFalcon.Domain.Model.Core;
using ArabyAds.AdFalcon.Domain.Repositories.Core;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Core;
using ArabyAds.AdFalcon.Services.Interfaces.Services;
using ArabyAds.AdFalcon.Services.Mapping;
using ArabyAds.Framework;

namespace ArabyAds.AdFalcon.Services.Services
{
    public class LocationService : ILocationService
    {
        private ILocationRepository locationRepository;
        private ICountryRepository cunotryRepository;
        public LocationService(ILocationRepository repository, ICountryRepository CountryRepository)
        {
            this.locationRepository = repository;

            this.cunotryRepository = CountryRepository;
        }

        public IEnumerable<Interfaces.DTOs.Core.LocationDto> GetAll()
        {
            //TODO:Osaleh to change this code , we should load the countries only
            //var countries = locationRepository.GetAll();
            // var list= countries.Select(locationDto => MapperHelper.Map<LocationDto>(locationDto)).ToList();

            return GetLocationFiltered();
            ///return list;
        
        }

        public IEnumerable<Interfaces.DTOs.Core.LocationDto> GetLocationFiltered()
        {
            var returnList = new List<Interfaces.DTOs.Core.LocationDto>();
            var list = locationRepository.GetAll();
            foreach (var item in list)
            {
                if (item.Parent == null)
                {
                    returnList.Add(MapperHelper.Map<LocationDto>(item));
                }
                else if (item.Parent is Country || item.Parent is Continent )
                {
                    var itemP = MapperHelper.Map<LocationDto>(item);
                    if (item.Parent is Country)
                        itemP.Locations = null;
                        returnList.Add(itemP);
                }
            }
        

            return returnList;
        }

      

        public List<Interfaces.DTOs.Core.LocationDto> GetContinentsByCountries(int[] Countries)
        {
            List<LocationDto> Continents = new List<LocationDto>();
            foreach (int id in Countries)
            {
                var Continent = locationRepository.Get(id).Parent;
                if (Continent != null)
                {
                    if (Continents.Where(x => x.ID == Continent.ID).Count() == 0)
                    {
                        Continents.Add(MapperHelper.Map<LocationDto>(Continent));
                    }
                }
            }

            return Continents;
        }
        public Interfaces.DTOs.Core.LocationDto GetCountryById(ValueMessageWrapper<int> Id)
        {
           
                var Country = locationRepository.Get(Id.Value);
            
                     return   MapperHelper.Map<LocationDto>(Country);
         
        }

        public IEnumerable<TreeDto> GetTree()
        {
            var returnList = new List<TreeDto>();
            var list = locationRepository.GetAll();

            returnList = BuildTreeView(list);

            return returnList;
        }

        public IEnumerable<TreeDto> GetTestTree()
        {
            var returnList = new List<TreeDto>();

            List<TreeDto> testtree = new List<TreeDto> {
     new TreeDto {
      Id = "1",
       Key = "p1",
       Name = LocalizedStringDto.ConvertToLocalizedStringDto("Pone"),
       Childs = new List < TreeDto > {
        new TreeDto {
         Id = "1",
          Key = "p1_1",

          Name = LocalizedStringDto.ConvertToLocalizedStringDto("Pone_1"),
          Childs = new List < TreeDto > {

          }
        },
        new TreeDto {
         Id = "2",
          Key = "p1_2",
          Name = LocalizedStringDto.ConvertToLocalizedStringDto("Pone_2"),
          Childs = new List < TreeDto > {

          }
        }
       }
     },// end
     new TreeDto {
      Id = "2",
       Key = "p2",
       Name = LocalizedStringDto.ConvertToLocalizedStringDto("PTwo"),
       Childs = new List < TreeDto > {
        new TreeDto {
         Id = "3",
          Key = "p2_1",
          Name = LocalizedStringDto.ConvertToLocalizedStringDto("Ptwo_1"),
          Childs = new List < TreeDto > {

          }
        },
        new TreeDto {
         Id = "4",
          Key = "p2_3",
          Name = LocalizedStringDto.ConvertToLocalizedStringDto("Ptwo_3"),
          Childs = new List < TreeDto > {

          }
        }
       }
     }// end 2
    
    };
            returnList = testtree;

            return returnList;
        }

        private List<TreeDto> BuildTreeView(IEnumerable<LocationBase> locations)
        {
            List<TreeDto> baseTreeView = new List<TreeDto>();

            foreach (var item in locations.Where(p => p.Parent == null))
            {
                var countryDto = new TreeDto { Id = item.ID.ToString(), Name = MapperHelper.Map<LocalizedStringDto>(item.Name) };
                BuildChildrenTreeView(countryDto, locations,item,1);
                baseTreeView.Add(countryDto);
            }

            return baseTreeView;
        }

        private void BuildChildrenTreeView(TreeDto parent, IEnumerable<LocationBase> locations, LocationBase pitem, int callno =1)
        {
            List<TreeDto> tree = new List<TreeDto>();
            IEnumerable<LocationBase> filteredItems = locations.Where(p => p.Parent != null && p.Parent.ID.ToString() == parent.Id);

            if (filteredItems != null && filteredItems.Count() != 0)
            {
                parent.Childs = new List<TreeDto>();
            }
             if (!(pitem is Country || pitem is Continent))
            {
                return;
            }
               /* if (callno==4)
            { return; }*/
            foreach (var item in filteredItems)
            {
                TreeDto child = new TreeDto
                {
                    Id = item.ID.ToString(),
                    Name = MapperHelper.Map<LocalizedStringDto>(item.Name)
                };
                ++callno;
                BuildChildrenTreeView(child, locations, item, callno);

                parent.Childs.Add(child);
            }
        }
    
    
    
    }
}
