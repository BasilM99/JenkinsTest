using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Campaign;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Reports;

namespace Noqoush.AdFalcon.Services.Interfaces.DTOs.AppSite
{
    [DataContract]
    public class AppSiteListResultDto
    {
        [DataMember]
        public IEnumerable<AppSiteListDto> Items { get; set; }
        [DataMember]
        public long TotalCount { get; set; }
    }



    [DataContract]
    public class SubAppSiteListResultDto
    {

       

        [DataMember]
        public List<SubAppsiteDto> Items { get; set; }
        [DataMember]
        public long TotalCount { get; set; }

    }
    [DataContract]
    public class AppSiteListResultDtoBase
    {
        [DataMember]
        public IEnumerable<AppSiteListDtoBase> Items { get; set; }
        [DataMember]
        public long TotalCount { get; set; }
    }

    [DataContract]
    public class AppSiteListDtoBase
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Type { get; set; }

        [DataMember]
        public string AccountName { get; set; }

        [DataMember]
        public int AccountId { get; set; }
    }

    [DataContract]
    public class AppSiteListDto : AppSiteListDtoBase
    {
        [DataMember]
        public string AccountName { get; set; }
        [DataMember]
        public string EmailAddress { get; set; }

        [DataMember]
        public string URL { get; set; }
        [DataMember]
        public string Type { get; set; }
        [DataMember]
        public DateTime RegistrationDate { get; set; }
        [DataMember]
        public string Status { get; set; }
        [DataMember]
        public string AdHouse { get; set; }
        [DataMember]
        public AppSitePerformance Performance { get; set; }

        [DataMember]
        public string TypeId { get; set; }

        [DataMember]
        public bool CantbeSelected { get; set; }

    }
}
