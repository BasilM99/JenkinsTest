using System;
using System.Linq.Expressions;
using Noqoush.AdFalcon.Domain.Common.Model.AppSite;

using System.Linq;
using Noqoush.AdFalcon.Domain.Common.Model.Campaign;
using Noqoush.AdFalcon.Domain.Common.Model.Account.SSP;
namespace Noqoush.AdFalcon.Domain.Common.Repositories.Account.SSP
{


    public class SiteZoneCriteria    {
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }

        public int BusinessId { get; set; }
        public int SiteId { get; set; }

        public int? Page { get; set; }
        public int Size { get; set; }
        public string ZoneName { get; set; }
        public string ZoneId { get; set; }
       
    }
}
