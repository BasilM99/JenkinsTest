using System.Collections.Generic;
using ProtoBuf;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Core;
using ArabyAds.Framework.DataAnnotations;

namespace ArabyAds.AdFalcon.Services.Interfaces.DTOs.AppSite
{
    [ProtoContract]
    public class AppSiteDtoBase
    {
        public AppSiteDtoBase()
        {

        }



       [ProtoMember(2)]
        public int ID { get; set; }

       [ProtoMember(3)]
        public string Name { get; set; }

       [ProtoMember(4)]
        public string PublisherId { get; set; }

       [ProtoMember(5)]
        public string URL { get; set; }

       [ProtoMember(6)]
        public bool IsPublished { get; set; }

       [ProtoMember(7)]
        public string Description { get; set; }

       [ProtoMember(8)]
        public IEnumerable<KeywordDto> Keywords { get; set; } = new List<KeywordDto>();

        [ProtoMember(9)]
        public string CurrentStatus { get; set; }

       [ProtoMember(10)]
        public int SubType { get; set; }

       [ProtoMember(11)]
        public int StatusId { get; set; }

       [ProtoMember(12)]
        public string AdminComment { get; set; }

        //TODO:Osaleh to remove this and get this info from more suitable source
       [ProtoMember(13)]
        public string AccountLanguage { get; set; }
        [ProtoMember(1)]
        public AppSiteAccountInfo AccountInfo { get; set; }
    }

    [ProtoContract]
    public class AppSiteAccountInfo
    {
        public AppSiteAccountInfo()
        {

        }

       [ProtoMember(1)]
        public string AccountName { get; set; }

       [ProtoMember(2)]
        public string AccountEmail { get; set; }

       [ProtoMember(3)]
        public string AccountCompanyName { get; set; }

       [ProtoMember(4)]
        public CountryDto Country { get; set; }
    }
}