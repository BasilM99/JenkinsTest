using ArabyAds.AdFalcon.Services.Interfaces.DTOs.AppSite;
using System;
using System.Collections.Generic;
using System.Text;

namespace ArabyAds.AdFalcon.Web.Controllers.Model.AppSite.Save
{
    public class SaveAppSiteFilterModel
    {
       public string appsiteId { get; set; }
            public string actionName { get; set; }
         public UrlFilterDto urlFilterDto { get; set; }
        public TextFilterDto textFilterDto { get; set; }
        public LanguageFilterDto languageFilterDto { get; set; }


         
    }
} 
