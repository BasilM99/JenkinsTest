using System.Collections.Generic;
using System.Runtime.Serialization;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Core;
using Noqoush.Framework.DataAnnotations;

namespace Noqoush.AdFalcon.Services.Interfaces.DTOs.AppSite
{
    [DataContract]
    public class AppSiteDtoBase
    {
        public AppSiteDtoBase()
        {

        }

        [DataMember]
        public AppSiteAccountInfo AccountInfo { get; set; }

        [DataMember]
        public int ID { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string PublisherId { get; set; }

        [DataMember]
        public string URL { get; set; }

        [DataMember]
        public bool IsPublished { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public IEnumerable<KeywordDto> Keywords { get; set; }

        [DataMember]
        public string CurrentStatus { get; set; }

        [DataMember]
        public int SubType { get; set; }

        [DataMember]
        public int StatusId { get; set; }

        [DataMember]
        public string AdminComment { get; set; }

        //TODO:Osaleh to remove this and get this info from more suitable source
        [DataMember]
        public string AccountLanguage { get; set; }
    }

    [DataContract]
    public class AppSiteAccountInfo
    {
        public AppSiteAccountInfo()
        {

        }

        [DataMember]
        public string AccountName { get; set; }

        [DataMember]
        public string AccountEmail { get; set; }

        [DataMember]
        public string AccountCompanyName { get; set; }

        [DataMember]
        public CountryDto Country { get; set; }
    }
}