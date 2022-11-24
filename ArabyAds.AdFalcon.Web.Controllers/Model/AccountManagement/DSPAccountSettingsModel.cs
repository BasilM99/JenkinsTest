using Noqoush.AdFalcon.Services.Interfaces.DTOs.Account;
using Noqoush.AdFalcon.Web.Controllers.Model.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Noqoush.AdFalcon.Web.Controllers.Model.AccountManagement
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
