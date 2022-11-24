using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ArabyAds.AdFalcon.Web.Controllers.Core.ViewComponents.Dashboard
{

    public class adperformance : ViewComponent
    {


        public adperformance()
        {

        }

        public async Task<IViewComponentResult> InvokeAsync(
       )
        {

            return View("adperformance");
        }

    }
}
