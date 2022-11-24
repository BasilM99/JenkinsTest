using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Account;
using ArabyAds.AdFalcon.Web.Controllers.Model.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
namespace ArabyAds.AdFalcon.Web.Controllers.Model.AccountManagement
{
    public class DSPAccountSettingsModel
    {
        public IList<SelectListItem> Countries { get; set; }

        public IList<SelectListItem> Cities { get; set; }

        public AccountDSPsettingDTO Setting { get; set; }

        public RecipientEmailModel Recipients { get; set; }

        public string RecipientsString { get; set; }

    }
}
