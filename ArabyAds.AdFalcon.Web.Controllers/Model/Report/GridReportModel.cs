using Noqoush.AdFalcon.Domain.Common.Model.Core;
using Noqoush.AdFalcon.Services.Interfaces.Core;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Reports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telerik.Web.Mvc.UI;

namespace Noqoush.AdFalcon.Web.Controllers.Model.Report
{
   
    public class GridReportModel
    {
        public List<metriceColumnDto> Columns { get; set; }
        public List<CampaignCommonReportDto> CampData { get; set; }
        public List<AppCommonReportDto> AppData { get; set; }
        public List<GridColumnSettings> GridColumnSettings { get ; set;}
    }

}
