using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ArabyAds.AdFalcon.Services.Interfaces.Services;
using ArabyAds.AdFalcon.Web.Controllers.Core;
using ArabyAds.Framework;

namespace ArabyAds.AdFalcon.Web.Controllers.Controllers
{
    public class ThemeController : AuthorizedControllerBase
    {
        private IThemeService _themeService;

        public ThemeController()
        {
            _themeService = IoC.Instance.Resolve<IThemeService>();
        }

        public ActionResult Index(string q)
        {
            var items = _themeService.GetAll();
            if (q != null)
            {

                return Json(items.Where(item => item.Name != null && item.Name.ToString().StartsWith(q, StringComparison.OrdinalIgnoreCase)));
            }
            return Json(items);
        }

        public ActionResult Item(int id)
        {
            var item = _themeService.Get(new ValueMessageWrapper<int> { Value = id });
            return Json(item);
        }

        public ActionResult Save(int id)
        {
            var item = _themeService.Get(new ValueMessageWrapper<int> { Value = id });
            return Json(item);
        }
        public ActionResult Themes()
        {
            var themesList = new List<SelectListItem>();
            List<ArabyAds.AdFalcon.Services.Interfaces.DTOs.Core.ThemeDto> ThemesDtos = _themeService.GetAll().ToList();
            foreach (var item in ThemesDtos)
            {
                var selectItem = new SelectListItem
                                     {
                                         Value = item.Id.ToString() + "!" + item.BackgroundColor + "!" + item.TextColor,
                                         Text = item.Name.ToString()
                                     };
                themesList.Add(selectItem);
            }
            return Json(themesList);
        }
    }
}

