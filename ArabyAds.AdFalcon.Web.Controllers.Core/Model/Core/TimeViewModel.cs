using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArabyAds.AdFalcon.Web.Controllers.Model.Core
{
    public class TimeViewModel
    {
        public string Name { get; set; }
        public string callBackFunction { get; set; }
        public int? Hour { get; set; }
        public int? Min { get; set; }
        public string action { get; set; }
        public string Code { get; set; }
    }

    public class TextViewModel
    {
        public string callBackFunction { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Value { get; set; }
        public string action { get; set; }
    }
}
