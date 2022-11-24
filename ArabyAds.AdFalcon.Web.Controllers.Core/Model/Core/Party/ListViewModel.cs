using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Core;
using ArabyAds.AdFalcon.Web.Controllers.Model.AppSite;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ArabyAds.AdFalcon.Web.Controllers.Model.Core.Party
{
    public class ListViewModel : ListViewModelBase
    {
        public IEnumerable<PartyDto> Items { get; set; }
        public string PartyType { get; set; }
        public IList<SelectListItem>  BusinessPartnerTypes { get; set; }
        
        public string SaveURL { get; set; }
       
    }
}

