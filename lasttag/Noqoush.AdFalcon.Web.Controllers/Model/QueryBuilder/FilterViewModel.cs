using Noqoush.AdFalcon.Web.Controllers.Model.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Noqoush.AdFalcon.Web.Controllers.Model.QueryBuilder
{
    public class FilterViewModel
    {
        public List<SelectListItem> Dimensions { get; set; }
        public TreeViewMode DimensionsTree { get; set; }
        public TreeViewMode MeasuresTree { get; set; }

        public List<SelectListItem> Facts { get; set; }
        public int FactId { get; set; }
        public Select2ViewModel Select2 { get; set; }
        public int Id { get; set; }
        public bool IncludeId { get; set; }
        public int DimensionsMaxSelect { get; set; }
        public  CampaignReportSchedulingViewModel SchedulingViewModel { get; set; }
        public string Dfrom { get; set; }
        public string Dto { get; set; }
        public string ReportTempName { get; set; }

    }
}