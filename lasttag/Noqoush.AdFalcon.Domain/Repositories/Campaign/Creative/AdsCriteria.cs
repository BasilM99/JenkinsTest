using System;
using System.Linq.Expressions;
using Noqoush.AdFalcon.Domain.Model.Core.CostElement;
using Noqoush.Framework.Persistence;

namespace Noqoush.AdFalcon.Domain.Repositories.Campaign.Creative
{
    public class AdGroupCostElementCriteria
    {
        public int CampaignId { get; set; }
        public int AdGroupId { get; set; }
        public DateTime? DataFrom { get; set; }
        public DateTime? DataTo { get; set; }
        public int Page { get; set; }
        public int Size { get; set; }

        public void CopyFromCommonToDomain(Noqoush.AdFalcon.Domain.Common.Repositories.Campaign.Creative.AdGroupCostElementCriteria Commoncr)
        {
            CampaignId = Commoncr.CampaignId;

            AdGroupId = Commoncr.AdGroupId;


            DataFrom = Commoncr.DataFrom; DataTo = Commoncr.DataTo;

            Page = Commoncr.Page;
            Size = Commoncr.Size;


        }
    }
}
