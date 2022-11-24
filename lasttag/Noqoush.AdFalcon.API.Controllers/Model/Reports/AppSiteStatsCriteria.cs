using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Noqoush.AdFalcon.API.Controllers.Model.Reports
{

    /// <summary>
    /// Represents the request parameters 
    /// </summary>
    public class AppSiteStatsCriteria
    {
        /// <summary>
        /// Determine if the data that will be is test data
        /// </summary>
        public bool IsTest { get; set; }

        /// <summary>
        /// Represents Response Format parameter.
        /// </summary>
        public string F { get; set; }

        /// <summary>
        /// Represents Start Date parameter, determines start date of the report.
        /// </summary>
        public string FDate { get; set; }

        /// <summary>
        /// Represents End Date parameter, determines end date of the report.
        /// </summary>
        public string TDate { get; set; }

        /// <summary>
        /// Represents Country Code parameter.
        /// </summary>
        public string CC { get; set; }

        /// <summary>
        /// Represents AppId parameter, the unique 32-character Id for the App/Site.
        /// </summary>
        public string AId { get; set; }

        /// <summary>
        /// Represents Length parameter, determines the maxiumum number of records returned in the response
        /// </summary>
        public int? L { get; set; }

        /// <summary>
        /// Represents Offset parameter, this parameter along with the length (l) are used to implement  pagination over large result set.
        /// </summary>
        public int OS { get; set; }

        /// <summary>
        /// Represents Groupby parameter, determines second level of data grouping.
        /// </summary>
        public string GB { get; set; }

     

    }
}
