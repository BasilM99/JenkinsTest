using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Noqoush.AdFalcon.Web.Controllers.Model
{
    public class BaseSearchInfo
    {
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public int? Page { get; set; }
        public int? Size { get; set; }
    }
}
