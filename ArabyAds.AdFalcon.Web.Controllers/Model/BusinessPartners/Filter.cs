using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Noqoush.AdFalcon.Web.Controllers.Model.BusinessPartners
{
    public class Filter
    {
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public int[] checkedRecords { get; set; }
        public int? page { get; set; }
        public int? size { get; set; }
        public int typeId { get; set; }
        public string name { get; set; }
    }
}
