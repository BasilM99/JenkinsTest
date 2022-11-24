using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.AppSite;

namespace Noqoush.AdFalcon.Web.Controllers.Model.AppSite
{
    public class AppSiteSettingUpdate : AppSiteUpdateBase
    {
        public SettingsDto SettingsDto { get; set; }
    }
}
