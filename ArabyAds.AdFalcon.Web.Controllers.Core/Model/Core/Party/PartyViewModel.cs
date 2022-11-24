using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Core;
using ArabyAds.Framework.DataAnnotations;
using ArabyAds.AdFalcon.Web.Controllers.Model.Campaign;
using Microsoft.AspNetCore.Mvc.Rendering;
namespace ArabyAds.AdFalcon.Web.Controllers.Model.Core.Party
{
    public class PartyViewModel
    {

        public PartyDto PartyDto { get; set; }
        public IList<SelectListItem> JobPositions { get; set; }
        public IList<SelectListItem> BusinessPartnerTypes { get; set; }
        public IEnumerable<SelectListItem> PriceModels { get; set; }
        public IEnumerable<SelectListItem> PriceEncryptionAlgorithm { get; set; }

        public string ViewName { get; set; }
        public int DemandType { get; set; }
        public CreativeUnitViewModel creativeUnit { get; set; }
        public int SupplyType { get; set; }
        public int DataProviderType { get; set; }
        public string SaveAction { get; set; }


    }
}
