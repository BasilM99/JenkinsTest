using System;
using System.Collections.Generic;
using System.Linq;
using ProtoBuf;
using System.Text;
using ArabyAds.AdFalcon.Domain.Common.Model.AppSite;
using ArabyAds.Framework.DataAnnotations;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Core;

namespace ArabyAds.AdFalcon.Services.Interfaces.DTOs.AppSite
{
    [ProtoContract]
    public class AppSiteServerSettingDto
    {
       [ProtoMember(1)]
        //[Required()]
        public int NativeLayoutId
        {
            get;
            set;
        }
       [ProtoMember(2)]
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

       [ProtoMember(3)]
        public bool AllowBlindAds { get; set; }
       [ProtoMember(4)]
        public bool GenerateSystemUniqueId { get; set; }
       [ProtoMember(5)]
        public ImpressionCountMode ImpressionCountMode { get; set; }
       [ProtoMember(6)]
        public string SupportedAdTypes { get; set; }
       [ProtoMember(7)]
        public string SupportedBannerImageTypes { get; set; }
       [ProtoMember(8)]
        public bool WatchTraffic { get; set; }
       [ProtoMember(9)]
        [Range(1, 2592000)]
        public int? AdRequestCacheLifeTime { get; set; }
        [ProtoMember(10)]
        public IList<AppSiteEventDto> Events { get; set; } = new List<AppSiteEventDto>();
       [ProtoMember(11)]
        public CostModelWrapperDto CostModelWrapper { get; set; }

       [ProtoMember(12)]
        public AppSitePlacementType PlacementType { get; set; }

       [ProtoMember(13)]
        [Range(0, 247483647)]
        public int? RewardedVideoItemValue { get; set; }
       [ProtoMember(14)]
        public string RewardedVideoItemName { get; set; }


    }
}
