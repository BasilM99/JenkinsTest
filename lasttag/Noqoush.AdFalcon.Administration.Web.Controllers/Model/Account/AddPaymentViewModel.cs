using System.Collections.Generic;
using System.Web.Mvc;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Account.Payment;

namespace Noqoush.AdFalcon.Administration.Web.Controllers.Model.Account
{
    public class AddPaymentViewModel
    {
        public NewPaymentDto PaymentDto { get; set; }
        public IEnumerable<SelectListItem> Currencies { get; set; }
        public IEnumerable<SelectListItem> PaymentTypes { get; set; }
        public int DocumentTypeId = 2;
    }
}
