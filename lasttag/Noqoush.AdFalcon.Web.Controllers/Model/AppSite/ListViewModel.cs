using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.AppSite;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Campaign;

namespace Noqoush.AdFalcon.Web.Controllers.Model.AppSite
{
    public class ListViewModel : ListViewModelBase
    {
        public IEnumerable<AppSiteListDto> Items { get; set; }
    }

}
