using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Noqoush.AdFalcon.Domain.Common.Model.AppSite;
using Noqoush.AdFalcon.Domain.Common.Model.Campaign;

namespace Noqoush.AdFalcon.Domain.Common.Repositories.Campaign
{
    public class CampaignCriteria 
    {
        public int AccountId { get; set; }

        public int? userId { get; set; }
        public bool ActiveCampaigns { get; set; }
        public bool IsPrimaryUser { get; set; }
        public DateTime? DataCreate { get; set; }
        public DateTime? DataFrom { get; set; }
        public DateTime? DataTo { get; set; }
        public CampaignType CampaignType { get; set; }
        public CampaignType OtherCampaignType { get; set; }
        //public int? StatusId { get; set; }
        public int? Page { get; set; }
        public int Size { get; set; }
        public int? AppSiteId { get; set; }
        public int? AdvertiserAccountId { get; set; }
        public int? AdvertiserId { get; set; }
        public string Name { get; set; }
        public CampaignCriteria()
        {
            CampaignType = CampaignType.Normal;
            OtherCampaignType = CampaignType.ProgrammaticGuaranteed;
        }
     
    }

    public class AllCampaignCriteria 
    {
        public int? AppSiteId { get; set; }
        public AllCampaignCriteria()
        {
        }
   
    }
}
