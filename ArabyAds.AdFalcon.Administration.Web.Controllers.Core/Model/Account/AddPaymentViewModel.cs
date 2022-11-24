using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Account.Payment;

namespace ArabyAds.AdFalcon.Administration.Web.Controllers.Model.Account
{
    public class AddPaymentViewModel
    {
        public string VATAmountPercentageString { get; set; }
        public decimal VATAmountPercentageValue { get; set; }

        public NewPaymentDto PaymentDto { get; set; }
        public IEnumerable<SelectListItem> Currencies { get; set; }
        public IEnumerable<SelectListItem> PaymentTypes { get; set; }
        public int DocumentTypeId = 2;
    }
}
