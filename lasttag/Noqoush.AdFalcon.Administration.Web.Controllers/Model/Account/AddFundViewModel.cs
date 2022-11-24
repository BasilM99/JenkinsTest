using System.Collections.Generic;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Account;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Account.Fund;
using System.Web.Mvc;

namespace Noqoush.AdFalcon.Administration.Web.Controllers.Model.Account
{
    public class AddFundViewModel
    {
        public NewFundDto FundDto { get; set; }
        public IEnumerable<SelectListItem> FundTypes { get; set; }
        public IEnumerable<SelectListItem> Currencies { get; set; }
        public IEnumerable<SelectListItem> Types { get; set; }
        public int DocumentTypeId = 3;
    }
}
