using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using ArabyAds.AdFalcon.Services.Interfaces.Services;
using ArabyAds.AdFalcon.Services.Interfaces.Services.AppSite;
using ArabyAds.AdFalcon.Services.Interfaces.Services.Core;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Core;

using System.IO;
using ArabyAds.AdFalcon.Services.Interfaces.Services.Reports;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Reports;
using ArabyAds.AdFalcon.Web.Controllers.Core;
using ArabyAds.Framework;
using ArabyAds.AdFalcon.Common.UserInfo;
using System.Drawing;
using ArabyAds.Framework.Utilities;

using iTextSharp.text.pdf;
using iTextSharp.text;
using ArabyAds.AdFalcon.Web.Controllers.Utilities;
using ArabyAds.AdFalcon.Services.Interfaces.Services.Campaign;
using ArabyAds.AdFalcon.Domain.Common.Repositories.Campaign;
using ArabyAds.AdFalcon.Web.Core.Helper;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Reports.Dashboard;
using ArabyAds.AdFalcon.Web.Controllers.Model;
using ArabyAds.AdFalcon.Services.Services;
using ArabyAds.Framework.Logging;
using ArabyAds.AdFalcon.Services.Interfaces.Services.Account.PMP;
using ArabyAds.AdFalcon.Web.Controllers.Core.Security;
using ArabyAds.AdFalcon.Domain.Common.Model.Account;
using ArabyAds.AdFalcon.Domain.Common.Model.Core;
using ArabyAds.AdFalcon.Services.Interfaces.Services.Account;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Campaign;
using System.Threading;
using Microsoft.AspNetCore.Http;
using Telerik.Web.Mvc;
using ArabyAds.AdFalcon.Services.Interfaces.Messages;
using System.Threading.Tasks;

namespace ArabyAds.AdFalcon.Web.Controllers.Core.ViewComponents.Party
{

    public class SSPSearch : ViewComponent
    {
        protected static IPartyService partyService;
        protected static ILookupService lookupService;

        static SSPSearch()
        {


            partyService = IoC.Instance.Resolve<IPartyService>();
            lookupService = IoC.Instance.Resolve<ILookupService>();

        }
        public SSPSearch()
        {
        }

      
        public async Task<IViewComponentResult> InvokeAsync(
       )
        {
          
            return View("SSPSearch", new ArabyAds.AdFalcon.Web.Controllers.Model.Core.Party.ListViewModel());
        }

    }
}
