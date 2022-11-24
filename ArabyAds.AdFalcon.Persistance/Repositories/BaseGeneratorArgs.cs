using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Reports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArabyAds.AdFalcon.Persistence.Reports.Repositories
{
    /// <summary>
    /// Represents the base for script generators arguments that will be used to generate script
    /// </summary>
    public class BaseGeneratorArgs
    {
        //Tables
        public string FactStatTable { get; set; }
        public string DayFactStatTable { get; set; }
        public string WeekFactStatTable { get; set; }
        public string MonthFactStatTable { get; set; }

        public ReportType ReportType { get; set; }
        public EntityType EntityType { get; set; }

    }
}
