using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Campaign.Creative;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArabyAds.AdFalcon.Web.Controllers.Model.Campaign
{
    public class AdCreativeFullSummaryViewModel
    {
        public AdCreativeFullSummaryDto ViewSummary { get; set; }

        public List<CreativeUnitViewModel> SnapshotViewModel { get; set; }
    }
}
