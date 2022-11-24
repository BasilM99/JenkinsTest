using Noqoush.AdFalcon.Services.Interfaces.DTOs.AppSite;
using Noqoush.Framework.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Noqoush.AdFalcon.Services.Interfaces.DTOs.Account.SSP
{

    [DataContract]
    public class SiteZoneMappingDto
    {
        [DataMember]
        public int ID { get; set; }

        [DataMember]
        public bool? IsInterstitial
        {
            get;
            set;
        }
        //[Required()]
        [DataMember]
        public int AdTypeID
        {
            get;
            set;
        }
        [DataMember]
        public string Description
        {
            get;
            set;
        }

             [DataMember]
        public string AdFalconSubPublisherId
        {
            get;
            set;
        }

        

        [DataMember]
       // [Required()]
        public int NativeLayoutId
        {
            get;
            set;
        }
        [DataMember]
        public bool IsNative
        {
            get;
            set;
        }
        [DataMember]
        public int ZoneID
        {
            get;
            set;
        }

        [DataMember]
        public int SiteID
        {
            get;
            set;
        }

        [DataMember]
        public string AdTypeString
        {
            get;
            set;
        }
        [DataMember]
        //[Required()]
        public int DeviceTypeID
        {
            get;
            set;
        }
        [DataMember]
        public string DeviceTypeString
        {
            get;
            set;
        }
        [DataMember]
        public int AppSiteID
        {
            get;
            set;
        }
        [DataMember]
        public string AppSiteString
        {
            get;
            set;
        }
        [DataMember]
        public AppSiteListResultDtoBase AppSiteDto
        {
            get;
            set;
        }


    }

    [DataContract]
    public class ResultSiteZoneMapping
    {
        [DataMember]
        public string ZoneIdStr { get; set; }
        [DataMember]
        public string SiteIdStr { get; set; }
        [DataMember]
        public int BusinessId { get; set; }
        [DataMember]
        public string BusinessName { get; set; }


        [DataMember]
        public int SiteId { get; set; }
        [DataMember]
        public string SiteName { get; set; }
        [DataMember]
        public int ZoneId { get; set; }
        [DataMember]
        public string ZoneName { get; set; }

        [DataMember]
        public List<SiteZoneMappingDto> Items { get; set; }
        [DataMember]
        public long TotalCount { get; set; }

    }
}
