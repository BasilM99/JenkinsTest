using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Campaign;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Reports;
using Noqoush.AdFalcon.Web.Controllers.Model.AppSite;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Core;
using System.Web.Mvc;
using Noqoush.AdFalcon.Domain.Common.Model.Account;

namespace Noqoush.AdFalcon.Web.Controllers.Model.Advertiser
{
    public class AdvertiserAssignmentModel
    {
        public int AdvertiserAccountId { get; set; }

        public bool IsRestricted { get; set; }
        public string AdvertiserName { get; set; }

        public string ReadUsers { get; set; }
        public string WriteUsers { get; set; }
        public AgencyCommission AgencyCommission { get; set; }

        public decimal AgencyCommissionValue { get; set; }
        public List<CustomSelectListItem> Users { get; set; }

    }

}
