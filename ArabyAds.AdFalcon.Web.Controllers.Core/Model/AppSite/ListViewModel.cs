using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.AppSite;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Campaign;

namespace ArabyAds.AdFalcon.Web.Controllers.Model.AppSite
{
    public class ListViewModel : ListViewModelBase
    {
        public IEnumerable<AppSiteListDto> Items { get; set; }
    }

}
