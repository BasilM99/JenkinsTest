using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Noqoush.AdFalcon.Services.Interfaces.Services.Campaign.Creative;
using Noqoush.AdFalcon.Web.Controllers.Core;
using Noqoush.AdFalcon.Web.Controllers.Model.Tree;
using System.Web.Mvc;
using Noqoush.AdFalcon.Services.Interfaces.Services.Core;
namespace Noqoush.AdFalcon.Web.Controllers.Controllers
{
    public class TileImageController : AuthorizedControllerBase
    {
        private ITileImageService _tileImageService;
        public TileImageController(ITileImageService tileImageService)
        {
            _tileImageService = tileImageService;
        }

        public JsonResult Index(string query)
        {
            var items = _tileImageService.GetAll();
            var result = new JsonResult { Data = items, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            return result;
        }
    }
}


