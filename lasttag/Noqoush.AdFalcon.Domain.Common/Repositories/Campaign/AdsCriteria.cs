using System;
using System.Linq.Expressions;
using Noqoush.AdFalcon.Domain.Common.Model.Core;
using System.Collections.Generic;
using System.Linq;

namespace Noqoush.AdFalcon.Domain.Common.Repositories.Campaign.Creative
{
    public class AdsCriteria 
    {
        public int CampaignId { get; set; }
        public int GroupId { get; set; }
        public DateTime? DataFrom { get; set; }
        public DateTime? DataTo { get; set; }
        public int? StatusId { get; set; }
        public int Page { get; set; }
        public string Name { get; set; }

        public int Size { get; set; }
        public List<int> Permissions
        {
            get;
            set;
        }
      
    }

    public class AdsSummaryCriteria 
    {
        public string AccountName { get; set; }
        public string CampaignName { get; set; }
        public string CompanyName { get; set; }
        public int CampaignId { get; set; }
        public int GroupId { get; set; }
        public int? StatusId { get; set; }
        public int? Account { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public int Page { get; set; }
        public int Size { get; set; }
     
    }
}
