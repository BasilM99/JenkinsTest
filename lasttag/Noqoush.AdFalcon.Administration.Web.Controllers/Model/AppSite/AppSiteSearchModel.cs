using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Noqoush.AdFalcon.Administration.Web.Controllers.Model.AppSite
{
    public class AppSiteSearchModel
    {
        public int? AccountId { get; set; }
        public bool? IgnoreIsPrimaryUser { get; set; }
 
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public int? TypeId { get; set; }
        public int Page { get; set; }
        public int AppSiteId { get; set; }
        public int Size { get; set; }
        public string Name { get; set; }

        public string AppSiteName { get; set; }

        public string SubPublisher { get; set; }
        public string Email { get; set; }
    }
}
