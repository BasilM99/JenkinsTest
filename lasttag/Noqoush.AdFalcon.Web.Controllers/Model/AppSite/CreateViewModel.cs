using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.AppSite;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Core;
using System.Web.Mvc;

namespace Noqoush.AdFalcon.Web.Controllers.Model.AppSite
{
    public class CreateViewModel
    {
        public string AppSiteViewName { get; set; }
        public KeywordViewModel KeywordViewModel { get; set; }
        public IEnumerable<AppSiteTypeDto> AppSiteTypes { get; set; }
        public AppSiteDto AppSiteDto { get; set; }
        public IEnumerable<Tab> Tabs { get; set; }
        public IEnumerable<SelectListItem> Themes { get; set; }
   }
    public class KeywordViewModel
    {
        //use this prefix for HTML control names
        public string Prefix { get; set; }
        public bool AllowInsert { get; set; }
        public bool AllowInclude { get; set; }
        public AutoComplete KewordAuto { get; set; }
        public IEnumerable<KeywordDto> Keywords { get; set; }
        public IEnumerable<TagCloud> KeywordTags { get; set; }
        public bool ExcludeSensitiveCategories { get; set; }
    }
}
