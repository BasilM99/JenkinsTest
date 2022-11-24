using System;
using System.Linq.Expressions;
using Noqoush.AdFalcon.Domain.Common.Model.AppSite;

using System.Linq;
using Noqoush.AdFalcon.Domain.Common.Model.Campaign;
using Noqoush.AdFalcon.Domain.Common.Model.Account.SSP;
namespace Noqoush.AdFalcon.Domain.Common.Repositories.Account.SSP
{


    public class FloorPriceCriteria 
    {
        public FloorPriceConfigType ConfigType { get; set; }
        public int SiteId { get; set; }
        public int ZoneId { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }

        public int? Page { get; set; }
        public int Size { get; set; }
        public string Name { get; set; }

        private string ConvertToString(object String)
        {

            return String.ToString();

        }
  
    }
}
