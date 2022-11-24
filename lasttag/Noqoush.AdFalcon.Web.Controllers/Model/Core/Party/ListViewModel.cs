using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Core;
using Noqoush.AdFalcon.Web.Controllers.Model.AppSite;
using System.Web.Mvc;

namespace Noqoush.AdFalcon.Web.Controllers.Model.Core.Party
{
    public class ListViewModel : ListViewModelBase
    {
        public IEnumerable<PartyDto> Items { get; set; }
        public string PartyType { get; set; }
        public IList<SelectListItem>  BusinessPartnerTypes { get; set; }
        
        public string SaveURL { get; set; }
       
    }
}

