using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using Noqoush.AdFalcon.Domain.Common.Model.AppSite;
using Noqoush.Framework.DataAnnotations;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Core;

namespace Noqoush.AdFalcon.Services.Interfaces.DTOs.AppSite
{
    [DataContract]
    public class AppSiteServerSettingDto
    {
        [DataMember]
        //[Required()]
        public int NativeLayoutId
        {
            get;
            set;
        }
        [DataMember]
        public bool IsNative
        {
            get
            {

                if (NativeLayoutId > 0)
                {
                    return true;
                }
                return false;
            }
            set { }
        }

        [DataMember]
        public bool AllowBlindAds { get; set; }
        [DataMember]
        public bool GenerateSystemUniqueId { get; set; }
        [DataMember]
        public ImpressionCountMode ImpressionCountMode { get; set; }
        [DataMember]
        public string SupportedAdTypes { get; set; }
        [DataMember]
        public string SupportedBannerImageTypes { get; set; }
        [DataMember]
        public bool WatchTraffic { get; set; }
        [DataMember]
        [Range(1, 2592000)]
        public int? AdRequestCacheLifeTime { get; set; }
        [DataMember]
        public IList<AppSiteEventDto> Events { get; set; }
        [DataMember]
        public CostModelWrapperDto CostModelWrapper { get; set; }

        [DataMember]
        public AppSitePlacementType PlacementType { get; set; }

        [DataMember]
        [Range(0, 247483647)]
        public int? RewardedVideoItemValue { get; set; }
        [DataMember]
        public string RewardedVideoItemName { get; set; }


    }
}
