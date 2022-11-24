using ArabyAds.AdFalcon.Services.Interfaces.DTOs.AppSite;
using ArabyAds.AdFalcon.Web.Controllers.Model.AppSite;
using System.Collections.Generic;

namespace ArabyAds.AdFalcon.Administration.Web.Controllers.Model.AppSite
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