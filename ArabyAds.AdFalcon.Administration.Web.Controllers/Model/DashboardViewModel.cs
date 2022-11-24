using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Noqoush.AdFalcon.Administration.Web.Controllers.Model
{
    public class DashboardViewModel
    {
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }

        public IList<SelectListItem> MetricsList { get; set; }
    }
}
