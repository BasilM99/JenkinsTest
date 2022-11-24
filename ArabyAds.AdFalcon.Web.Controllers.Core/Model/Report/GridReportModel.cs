using ArabyAds.AdFalcon.Domain.Common.Model.Core;
using ArabyAds.AdFalcon.Services.Interfaces.Core;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Reports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kendo.Mvc.UI;

namespace ArabyAds.AdFalcon.Web.Controllers.Model.Report
{
   
    public class GridReportModel
    {
        public List<metriceColumnDto> Columns { get; set; }
        public List<CampaignCommonReportDto> CampData { get; set; }
        public List<AppCommonReportDto> AppData { get; set; }
        public List<GridColumnSettings> GridColumnSettings { get ; set;}
    }

}
