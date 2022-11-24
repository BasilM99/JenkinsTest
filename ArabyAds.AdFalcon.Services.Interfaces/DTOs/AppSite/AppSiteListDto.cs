using System;
using ProtoBuf;
using System.Collections.Generic;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Campaign;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Reports;

namespace ArabyAds.AdFalcon.Services.Interfaces.DTOs.AppSite
{
    [ProtoContract]
    public class AppSiteListResultDto
    {
       [ProtoMember(1)]
        public IEnumerable<AppSiteListDto> Items { get; set; } = new List<AppSiteListDto>();
        [ProtoMember(2)]
        public long TotalCount { get; set; }
    }



    [ProtoContract]
    public class SubAppSiteListResultDto
    {

       

       [ProtoMember(1)]
        public List<SubAppsiteDto> Items { get; set; } = new List<SubAppsiteDto>();
        [ProtoMember(2)]
        public long TotalCount { get; set; }

    }
    [ProtoContract]
    public class AppSiteListResultDtoBase
    {
       [ProtoMember(1)]
        public IEnumerable<AppSiteListDtoBase> Items { get; set; } = new List<AppSiteListDtoBase>();
        [ProtoMember(2)]
        public long TotalCount { get; set; }
    }

    [ProtoContract]
    [ProtoInclude(100,typeof(AppSiteListDto))]
    public class AppSiteListDtoBase
    {
       [ProtoMember(1)]
        public int Id { get; set; }

       [ProtoMember(2)]
        public string Name { get; set; }

       [ProtoMember(3)]
        public string Type { get; set; }

       [ProtoMember(4)]
        public string AccountName { get; set; }

       [ProtoMember(5)]
        public int AccountId { get; set; }
    }

    [ProtoContract]
    public class AppSiteListDto : AppSiteListDtoBase
    {

       [ProtoMember(1)]
        public string EmailAddress { get; set; }

       [ProtoMember(2)]
        public string URL { get; set; }

       [ProtoMember(3)]
        public DateTime RegistrationDate { get; set; }
       [ProtoMember(4)]
        public string Status { get; set; }
       [ProtoMember(5)]
        public string AdHouse { get; set; }
       [ProtoMember(6)]
        public AppSitePerformance Performance { get; set; }

       [ProtoMember(7)]
        public string TypeId { get; set; }

       [ProtoMember(8)]
        public bool CantbeSelected { get; set; }

        [ProtoMember(9)]
        public string StatusId { get; set; }
    }
}
