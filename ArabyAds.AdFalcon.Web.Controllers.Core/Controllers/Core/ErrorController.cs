using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using ArabyAds.AdFalcon.Web.Controllers.Core;
using ControllerBase = ArabyAds.AdFalcon.Web.Controllers.Core.ControllerBase;

namespace ArabyAds.AdFalcon.Web.Controllers.Controllers.Core
{
    //[HandleException]
    //[Route("error")]
   // [RequireHttps(Order = 1)]
    public class ErrorController : ControllerBase
    {


        public IActionResult Error(int? statusCode = null)
        {
            if (statusCode.HasValue)
            {
              //  Log.Error($"Error statusCode: {statusCode}");
                if (statusCode == 403)
                {
                    return View("NotFound");
                }
                if (statusCode == 404)
                {
                    return View("NotFound");
                }
            }

            return View();
        }

        //public   NoveeotFoundResult NotFound()
        //{
        //    Response.StatusCode = (int)HttpStatusCode.NotFound;
        //    //Response.TrySkipIisCustomErrors = true;
        //    return NotFoundResult();
        //}
        // [Route("500")]
        [Route("error")]

        public ActionResult Error()
        {
           // Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            // Response.TrySkipIisCustomErrors = true;
            return View();
        }
        //[Route("404")]
        public   ActionResult ItemNotFound()
        {
            //Response.StatusCode = (int)HttpStatusCode.NotFound;
           // Response.TrySkipIisCustomErrors = true;
            return View();
        }
    }
}
