using Noqoush.AdFalcon.Services.Interfaces.DTOs.Reports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Noqoush.AdFalcon.Persistence.Reports.RepositoriesGP
{
    /// <summary>
    /// Represents the base for script generators arguments that will be used to generate script
    /// </summary>
    public class BaseGeneratorArgs
    {
        //Tables

        public string OrderByStruct { get; set; }
        public string FactStatTable { get; set; }
        public string DayFactStatTable { get; set; }
        public string WeekFactStatTable { get; set; }
        public string MonthFactStatTable { get; set; }
        public string DropStatements { get; set; }
        public ReportType ReportType { get; set; }
        public EntityType EntityType { get; set; }

    }
}
