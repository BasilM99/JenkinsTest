using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Core;
using Noqoush.AdFalcon.Web.Controllers.Model.AppSite;

namespace Noqoush.AdFalcon.Administration.Web.Controllers.Model.Core.Party
{
    public class ListViewModel : ListViewModelBase
    {
        public IEnumerable<PartyDto> Items { get; set; }
        public string PartyType { get; set; }
        public string SaveURL { get; set; }
       
    }
}

