using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArabyAds.AdFalcon.Web.Controllers.Model.AppSite
{
    public class AppSiteCriteriaModel
    {
        public int? FilterType { get; set; }

        public int? Page { get; set; }

        public int? Size { get; set; }

        public int? AppId { get; set; }

        public DateTime? FromDate { get; set; }

        public DateTime? ToDate { get; set; }

        public int? AccountId { get; set; }


    }
}
