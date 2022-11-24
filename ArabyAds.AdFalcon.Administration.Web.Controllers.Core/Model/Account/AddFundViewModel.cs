using System.Collections.Generic;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Account;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Account.Fund;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ArabyAds.AdFalcon.Domain.Common.Model.Account;

namespace ArabyAds.AdFalcon.Administration.Web.Controllers.Model.Account
{
    public class AddFundViewModel
    {
        public string VATAmountPercentageString { get; set; }

        public decimal VATAmountPercentageValue { get; set; }
        public NewFundDto FundDto { get; set; }
        public IEnumerable<SelectListItem> FundTypes { get; set; }
        public IEnumerable<SelectListItem> Currencies { get; set; }
        public IEnumerable<SelectListItem> Types { get; set; }
        public int DocumentTypeId = 3;

        public  PayemntAccountType accountType {get;set;}
        public  int accountId {get;set;}

    }
}
