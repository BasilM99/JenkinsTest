using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNetCore.Mvc;

namespace ArabyAds.AdFalcon.Web.Controllers.Model.QueryBuilder
{

    public class Select2ViewModel
    {
        public string Code { get; set; }
        public string ControllerName { get; set; }
        public string ActionName { get; set; }
        public string Id { get; set; }
        public string ListOfIds { get; set; }
        public string OptionalParameter { get; set; }
        public string callBackFunction { get; set; }
        public string LookUpName { get; set; }
    }
}