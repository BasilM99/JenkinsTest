using System;
using System.Collections.Generic;
using System.Linq;
using ProtoBuf;
using System.Text;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Core;
using ArabyAds.Framework.DataAnnotations;

namespace ArabyAds.AdFalcon.Services.Interfaces.DTOs.AppSite
{
    [ProtoContract]
    public class AppSiteDto
    {
       [ProtoMember(1)]
        public int ID { get; set; }

       [ProtoMember(2)]
        [Required()]
        [StringLength(255)]
        public string Name
        {
            get;
            set;
        }

       [ProtoMember(3)]
        public string PublisherId
        {
            get;
            set;
        }

       [ProtoMember(4)]
        [StringLength(255)]
        public string URL { get; set; }

       [ProtoMember(5)]
        [Required]
        public bool IsPublished { get; set; }

       [ProtoMember(6)]
        [StringLength(1024)]
        public string Description { get; set; }

       [ProtoMember(7)]
        public int SubType { get; set; }


       [ProtoMember(8)]
        public string AdminComment { get; set; }
       [ProtoMember(9)]
        public string EmailText { get; set; }

       [ProtoMember(10)]
        public AppSiteTypeDto Type { get; set; }

       [ProtoMember(11)]
        public ThemeDto Theme { get; set; }

       [ProtoMember(12)]
        public IEnumerable<KeywordDto> Keywords { get; set; } = new List<KeywordDto>();

       [ProtoMember(13)]
        public string[] NewKeywords { get; set; }

       [ProtoMember(14)]
        public string[] DeletedKeywords { get; set; }

       [ProtoMember(15)]
        public string SupUserName { get; set; }

       [ProtoMember(16)]
        public int PlacementType { get; set; }

       [ProtoMember(17)]
        //[Range(0, 247483647)]
        public int? RewardedVideoItemValue { get; set; }
       [ProtoMember(18)]
        //[StringLength(255)]
        public string RewardedVideoItemName { get; set; }


        [ProtoMember(19)]
        public IEnumerable<int> intKeywords { get; set; } = new List<int>();

        [ProtoMember(20)]
        public string AccountLanguage { get; set; }
        [ProtoMember(21)]
        public AppSiteAccountInfo AccountInfo { get; set; }

        [ProtoMember(22)]
        public string CurrentStatus { get; set; }

    }
}
