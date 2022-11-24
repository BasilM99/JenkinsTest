using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Reports;
using System;
using System.Collections.Generic;
using System.Text;

namespace ArabyAds.AdFalcon.Web.Controllers.Model
{
   

    public class DashBoardFilterModel
    {

        public ReportCriteriaDto dto { get; set; }
        public int SectionType { get; set; }
        public string q { get; set; }

        public int UserId { get; set; }

        public int? AdvertiserAccountId { get; set; }
        public int? AdvertiserId { get; set; }
        public int? dealId { get; set; }
  
    }
}
