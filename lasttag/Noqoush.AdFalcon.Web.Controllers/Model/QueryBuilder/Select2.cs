using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Noqoush.AdFalcon.Web.Controllers.Model.QueryBuilder
{

    public class Select2
    {
        public long TotalCount { get; set; }
        public int id { get; set; }
        public string text { get; set; }
        public IList<Select2> children { get; set; }
    }
}