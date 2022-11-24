using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Campaign;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Reports;
using ArabyAds.AdFalcon.Web.Controllers.Model.AppSite;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Core;
using Microsoft.AspNetCore.Mvc;
using ArabyAds.AdFalcon.Domain.Common.Model.Account;

namespace ArabyAds.AdFalcon.Web.Controllers.Model.Advertiser
{
    public class AdvertiserAssignmentModel
    {
        public int AdvertiserAccountId { get; set; }

        public bool IsRestricted { get; set; }
        public string AdvertiserName { get; set; }

        public string ReadUsers { get; set; }
        public string WriteUsers { get; set; }
        public AgencyCommission AgencyCommission { get; set; }
        public int AgencyCommissionInt { get => (int)AgencyCommission; set { } }

        public decimal AgencyCommissionValue { get; set; }
        public List<CustomSelectListItem> Users { get; set; }

    }

}
