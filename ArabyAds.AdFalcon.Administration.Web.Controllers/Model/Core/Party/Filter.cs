using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Noqoush.AdFalcon.Administration.Web.Controllers.Model.Core.Party
{
    public class Filter
    {
        public int? page { get; set; }
        public int? size { get; set; }
        public string id { get; set; }
        public string Prefix { get; set; }
    }
}
