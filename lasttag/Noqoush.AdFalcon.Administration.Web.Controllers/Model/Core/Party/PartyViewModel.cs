using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Core;
using Noqoush.Framework.DataAnnotations;

namespace Noqoush.AdFalcon.Administration.Web.Controllers.Model.Core.Party
{
    public class PartyViewModel
    {

        public PartyDto PartyDto { get; set; }
        public IList<SelectListItem> JobPositions { get; set; }
        public IList<SelectListItem> BusinessPartnerTypes { get; set; }
        public string ViewName { get; set; }
        public int DemandType { get; set; }

        public int SupplyType { get; set; }
        public string SaveAction { get; set; }


    }
}
