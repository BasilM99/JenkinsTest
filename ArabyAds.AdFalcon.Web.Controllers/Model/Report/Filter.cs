using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Noqoush.AdFalcon.Web.Controllers.Model.Report
{
   
    public class Filter
    {
        //public int? StatusId { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public int[] checkedRecords { get; set; }
        public int? page { get; set; }
        public int? size { get; set; }
    }
}
