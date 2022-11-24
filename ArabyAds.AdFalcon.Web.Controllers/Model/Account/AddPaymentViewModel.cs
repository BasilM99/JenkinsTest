using System.Collections.Generic;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Account.Payment;
using System.Web.Mvc;

namespace Noqoush.AdFalcon.Web.Controllers.Model.Account
{
    public class AddPaymentViewModel
    {
        public NewPaymentDto PaymentDto { get; set; }
        public IEnumerable<SelectListItem> Accounts { get; set; }
        public IEnumerable<SelectListItem> PaymentTypes { get; set; }
        public int DocumentTypeId = 1;
    }
}
