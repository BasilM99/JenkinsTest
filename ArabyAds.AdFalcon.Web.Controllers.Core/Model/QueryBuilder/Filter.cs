using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNetCore.Mvc;

namespace ArabyAds.AdFalcon.Web.Controllers.Model.QueryBuilder
{
    public class Filter
    {
        public int id { set; get; }
        public string Name { set; get; }
        public string Query { set; get; }

    }
}