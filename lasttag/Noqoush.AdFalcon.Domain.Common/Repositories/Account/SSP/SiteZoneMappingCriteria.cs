using System;
using System.Linq.Expressions;
using Noqoush.AdFalcon.Domain.Common.Model.AppSite;

using System.Linq;
using Noqoush.AdFalcon.Domain.Common.Model.Campaign;
using Noqoush.AdFalcon.Domain.Common.Model.Account.SSP;
namespace Noqoush.AdFalcon.Domain.Common.Repositories.Account.SSP
{


    public class SiteZoneMappingCriteria 
    {


        public string AdFalconSubPublisherId { get; set; }
        public string AppSiteName { get; set; }
        public int? AdTypeId { get; set; }
        public int? DeviceTypeId { get; set; }
        public int AppSiteId { get; set; }
        public bool? IsInterstitial { get; set; }
        public int ZoneId { get; set; }
        public int SiteId { get; set; }
        public int BusinessId { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }

        public int? Page { get; set; }
        public int Size { get; set; }
        public string MappingName { set; get; }
       
    }
}
