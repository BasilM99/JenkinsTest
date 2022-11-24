using ArabyAds.AdFalcon.Domain.Common.Model.Core;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Reports;
using ArabyAds.AdFalcon.Web.Controllers.Model.Report;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
namespace ArabyAds.AdFalcon.Web.Controllers.Model.Core
{
    public class CampaignReportSchedulingViewModel: RecipientEmailModel
    {

        public ReportSchedulerDto ReportSchedulerDto { set; get; }
        public List<ReportCriteriaSchedulerDto> CriteriaItems { get; set; }
        public ColumnViewModel ColumnViewModel { get; set; }
        public IList<SelectListItem> Time { set; get; }
        public IList<SelectListItem> Months { set; get; }
        public IList<SelectListItem> Days { set; get; }
        public IList<SelectListItem> Weeks { get; set; }

        public WeekDay test { get; set; }
        public string Subject { get; set; }

    }

    public class RecipientEmailModel {


        public IList<string> RecipientEmail { get; set; }



    }
}
