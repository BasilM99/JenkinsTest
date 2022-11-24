using ArabyAds.AdFalcon.Services.Interfaces.DTOs.AppSite;
using ArabyAds.Framework.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using ProtoBuf;
using System.Text;
using System.Threading.Tasks;

namespace ArabyAds.AdFalcon.Services.Interfaces.DTOs.Account.SSP
{

    [ProtoContract]
    public class SiteZoneMappingDto
    {
       [ProtoMember(1)]
        public int ID { get; set; }

       [ProtoMember(2)]
        public bool? IsInterstitial
        {
            get;
            set;
        }
        //[Required()]
       [ProtoMember(3)]
        public int AdTypeID
        {
            get;
            set;
        }
       [ProtoMember(4)]
        public string Description
        {
            get;
            set;
        }

            [ProtoMember(5)]
        public string AdFalconSubPublisherId
        {
            get;
            set;
        }

        

       [ProtoMember(6)]
       // [Required()]
        public int NativeLayoutId
        {
            get;
            set;
        }
       [ProtoMember(7)]
        public bool IsNative
        {
            get;
            set;
        }
       [ProtoMember(8)]
        public int ZoneID
        {
            get;
            set;
        }

       [ProtoMember(9)]
        public int SiteID
        {
            get;
            set;
        }

       [ProtoMember(10)]
        public string AdTypeString
        {
            get;
            set;
        }
       [ProtoMember(11)]
        //[Required()]
        public int DeviceTypeID
        {
            get;
            set;
        }
       [ProtoMember(12)]
        public string DeviceTypeString
        {
            get;
            set;
        }
       [ProtoMember(13)]
        public int AppSiteID
        {
            get;
            set;
        }
       [ProtoMember(14)]
        public string AppSiteString
        {
            get;
            set;
        }
       [ProtoMember(15)]
        public AppSiteListResultDtoBase AppSiteDto
        {
            get;
            set;
        }


    }

    [ProtoContract]
    public class ResultSiteZoneMapping
    {
       [ProtoMember(1)]
        public string ZoneIdStr { get; set; }
       [ProtoMember(2)]
        public string SiteIdStr { get; set; }
       [ProtoMember(3)]
        public int BusinessId { get; set; }
       [ProtoMember(4)]
        public string BusinessName { get; set; }


       [ProtoMember(5)]
        public int SiteId { get; set; }
       [ProtoMember(6)]
        public string SiteName { get; set; }
       [ProtoMember(7)]
        public int ZoneId { get; set; }
       [ProtoMember(8)]
        public string ZoneName { get; set; }

        [ProtoMember(9)]
        public List<SiteZoneMappingDto> Items { get; set; } = new List<SiteZoneMappingDto>();
       [ProtoMember(10)]
        public long TotalCount { get; set; }

    }
}
