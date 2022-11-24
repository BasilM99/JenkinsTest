using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArabyAds.AdFalcon.Services.Interfaces.Services.Campaign.Creative;
using ArabyAds.AdFalcon.Web.Controllers.Core;
using ArabyAds.AdFalcon.Web.Controllers.Model.Tree;
using Microsoft.AspNetCore.Mvc;
using ArabyAds.AdFalcon.Services.Interfaces.Services.Core;
using ArabyAds.Framework;

namespace ArabyAds.AdFalcon.Web.Controllers.Controllers
{
    public class TileImageController : AuthorizedControllerBase
    {
        private ITileImageService _tileImageService;
        public TileImageController()
        {
            _tileImageService = IoC.Instance.Resolve<ITileImageService>();
        }

        public JsonResult Index(string query)
        {
            var items = _tileImageService.GetAll();
            var result = new JsonResult (  items);
            return result;
        }
    }
}


