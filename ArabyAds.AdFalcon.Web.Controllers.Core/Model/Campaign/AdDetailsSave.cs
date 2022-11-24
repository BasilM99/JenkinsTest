using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Campaign;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Campaign.Creative;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace ArabyAds.AdFalcon.Web.Controllers.Model.Campaign
{
    public class AdDetailsSave
    {
        public int id { get; set; }
        public int adGroupId { get; set; }
        public int  adId { get; set; }
        public string[] appSiteId { get; set; }
        public string[] deletedAppSiteId { get; set; }
        public string RunType { get; set; }

        public bool Include { get; set; }
        public string DomainURL { get; set; }
        public string adsToCopyAppsites { get; set; }
        public KeywordDto Keyword { get; set; }
        public string InsertedAppsites { get; set; }
        public string KeywordId { get; set; }
        public string LanguageId { get; set; }

        public bool Reject { get; set; }

        public IList<AdCreativeUnitDto> CreativeUnitsContent { get; set; }


        public IList<CampaignBidConfigDto> UpdatedCampaignBidConfigDtos { get; set; }
    }
}
