using Noqoush.AdFalcon.Services.Interfaces.DTOs.Reports;
using Noqoush.AdFalcon.Web.Controllers.Model.AppSite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Noqoush.AdFalcon.Web.Controllers.Model.Report
{
   
    public class ReportJobListViewModelBase : ListViewModelBase
    {
        public IEnumerable<SelectListItem> Statuses { get; set; }
    }
    public class ListViewModel : ReportJobListViewModelBase
    {
        public IEnumerable<ReportSchedulerDto> Items { get; set; }

    }

}
