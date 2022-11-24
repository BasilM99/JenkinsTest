using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Noqoush.AdFalcon.Web.Controllers.Model
{

    public class Select2ViewModel
    {

        public string ControllerName { get; set; }
        public string ActionName { get; set; }

        public string PlaceHolder { get; set; }
        public string Id { get; set; }
        public string OptionalParameter { get; set; }
        public string OnReadyFunctions { get; set; }
        public string OnSelectFunctions { get; set; }
        public bool disabled { get; set; }
        public bool Single { get; set; }
        public bool AllowClear { get; set; }
        public bool IsServerSide { get; set; }
        public bool IsTree { get; set; }
        public string ClintSideResourceFunction { get; set; }
        public Object ParameterObject { get; set; }
    }
}