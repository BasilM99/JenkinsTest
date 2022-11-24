using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Core;
using Noqoush.Framework.DataAnnotations;

namespace Noqoush.AdFalcon.Services.Interfaces.DTOs.AppSite
{
    [DataContract]
    public class AppSiteDto
    {
        [DataMember]
        public int ID { get; set; }

        [DataMember]
        [Required()]
        [StringLength(255)]
        public string Name
        {
            get;
            set;
        }

        [DataMember]
        public string PublisherId
        {
            get;
            set;
        }

        [DataMember]
        [StringLength(255)]
        public string URL { get; set; }

        [DataMember]
        [Required]
        public bool IsPublished { get; set; }

        [DataMember]
        [StringLength(1024)]
        public string Description { get; set; }

        [DataMember]
        public int SubType { get; set; }


        [DataMember]
        public string AdminComment { get; set; }
        [DataMember]
        public string EmailText { get; set; }

        [DataMember]
        public AppSiteTypeDto Type { get; set; }

        [DataMember]
        public ThemeDto Theme { get; set; }

        [DataMember]
        public IEnumerable<KeywordDto> Keywords { get; set; }

        [DataMember]
        public string[] NewKeywords { get; set; }

        [DataMember]
        public string[] DeletedKeywords { get; set; }

        [DataMember]
        public string SupUserName { get; set; }

        [DataMember]
        public int PlacementType { get; set; }

        [DataMember]
        //[Range(0, 247483647)]
        public int? RewardedVideoItemValue { get; set; }
        [DataMember]
        //[StringLength(255)]
        public string RewardedVideoItemName { get; set; }

    }
}
