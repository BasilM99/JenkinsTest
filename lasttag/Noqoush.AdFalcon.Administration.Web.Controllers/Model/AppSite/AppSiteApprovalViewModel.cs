using Noqoush.AdFalcon.Services.Interfaces.DTOs.AppSite;
using Noqoush.AdFalcon.Web.Controllers.Model.AppSite;
using System.Collections.Generic;

namespace Noqoush.AdFalcon.Administration.Web.Controllers.Model.AppSite
{
    public class AppSiteApprovalViewModel : AppSiteUpdateBase
    {
        public IEnumerable<AppSiteTypeDto> AppSiteTypes { get; set; }
        public KeywordViewModel KeywordViewModel { get; set; }
        public string AppSiteViewName { get; set; }
        public string ApprovalViewName { get; set; }
        public AppSiteDtoBase AppSite { get; set; }

        public string Comments { get; set; } 
    }
}