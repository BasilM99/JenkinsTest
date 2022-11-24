using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Text;

namespace ArabyAds.AdFalcon.Web.Controllers
{
    

    [AttributeUsage( AttributeTargets.Method, Inherited = true, AllowMultiple = false)]

    public class OutputCacheAttribute : ResponseCacheAttribute
    {
      
    }
}
