using System;
using System.Linq.Expressions;
using Noqoush.AdFalcon.Domain.Common.Model.Core.CostElement;


namespace Noqoush.AdFalcon.Domain.Common.Repositories.Campaign.Creative
{
    public class AdGroupCostElementCriteria
    {
        public int CampaignId { get; set; }
        public int AdGroupId { get; set; }
        public DateTime? DataFrom { get; set; }
        public DateTime? DataTo { get; set; }
        public int Page { get; set; }
        public int Size { get; set; }
    }
}
