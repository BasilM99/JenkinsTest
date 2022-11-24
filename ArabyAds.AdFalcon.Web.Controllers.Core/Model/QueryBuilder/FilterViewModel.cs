using ArabyAds.AdFalcon.Web.Controllers.Model.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
namespace ArabyAds.AdFalcon.Web.Controllers.Model.QueryBuilder
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


        public decimal Multiplier { get; set; }
        public string DimensionValue { get; set; }
        public int DimensionType { get; set; }
        public int BidModId { get; set; }
        public int Index { get; set; }
        public string CriteriaIDs { get; internal set; }
        public string SearchstringCriteriaIDs { get; internal set; }
        public string ConfigForMeasureDimensionFilter { get; internal set; }
    }
}