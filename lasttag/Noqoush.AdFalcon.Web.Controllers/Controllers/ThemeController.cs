using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Noqoush.AdFalcon.Services.Interfaces.Services;
using Noqoush.AdFalcon.Web.Controllers.Core;
using Noqoush.Framework;

namespace Noqoush.AdFalcon.Web.Controllers.Controllers
{
    public class ThemeController : AuthorizedControllerBase
    {
        private IThemeService _themeService;

        public ThemeController(IThemeService themeService)
        {
            _themeService = themeService;
        }

        public ActionResult Index(string q)
        {
            var items = _themeService.GetAll();
            if (q != null)
            {

                return Json(items.Where(item => item.Name != null && item.Name.ToString().StartsWith(q, StringComparison.OrdinalIgnoreCase)),
                    JsonRequestBehavior.AllowGet);
            }
            return Json(items, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Item(int id)
        {
            var item = _themeService.Get(id);
            return Json(item, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Save(int id)
        {
            var item = _themeService.Get(id);
            return Json(item, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Themes()
        {
            var themesList = new List<SelectListItem>();
            List<Noqoush.AdFalcon.Services.Interfaces.DTOs.Core.ThemeDto> ThemesDtos = _themeService.GetAll().ToList();
            foreach (var item in ThemesDtos)
            {
                var selectItem = new SelectListItem
                                     {
                                         Value = item.Id.ToString() + "!" + item.BackgroundColor + "!" + item.TextColor,
                                         Text = item.Name.ToString()
                                     };
                themesList.Add(selectItem);
            }
            return Json(themesList, JsonRequestBehavior.AllowGet);
        }
    }
}

