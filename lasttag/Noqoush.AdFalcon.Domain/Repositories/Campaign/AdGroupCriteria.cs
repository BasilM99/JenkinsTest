using System;
using System.Linq.Expressions;
using Noqoush.AdFalcon.Domain.Model.AppSite;
using Noqoush.Framework.Persistence;
using System.Linq;
using Noqoush.AdFalcon.Domain.Model.Campaign;
using System.Collections.Generic;
using Noqoush.AdFalcon.Domain.Model.Core;
using Noqoush.Framework;
using Noqoush.AdFalcon.Domain.Model.Account;
using Noqoush.AdFalcon.Domain.Common.Model.Campaign;

namespace Noqoush.AdFalcon.Domain.Repositories.Campaign
{
    public class AdGroupCriteria : CriteriaBase<Model.Campaign.AdGroup>
    {

        //   private IAccountAdPermissionsRepository AdPermissionsRepository = IoC.Instance.Resolve<IAccountAdPermissionsRepository>();

        public int CampaignId { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public string Name { get; set; }
        public int Page { get; set; }
        public int Size { get; set; }
        public int AccountId { get; set; }

        public int? AppSiteId { get; set; }

        public List<int> Permissions
        {
            get;
            set;
        }



        public void CopyFromCommonToDomain(Noqoush.AdFalcon.Domain.Common.Repositories.Campaign.AdGroupCriteria Commoncr)
        {
            CampaignId = Commoncr.CampaignId;

            AccountId = Commoncr.AccountId;
            Name = Commoncr.Name;
            AppSiteId = Commoncr.AppSiteId;
            Permissions = Commoncr.Permissions;
            DateFrom = Commoncr.DateFrom; DateTo = Commoncr.DateTo;

            Page = Commoncr.Page;
            Size = Commoncr.Size;


        }
        public override Expression<Func<Model.Campaign.AdGroup, bool>> GetExpression()
        {
            if (Name == null)
            {
                Name = string.Empty;
            }
            Expression<Func<Model.Campaign.AdGroup, bool>> filter = (c => !c.IsDeleted
                  && (c.Campaign.CampaignType == CampaignType.Normal  || c.Campaign.CampaignType == CampaignType.ProgrammaticGuaranteed) && (!AppSiteId.HasValue || c.Ads.Any(ad => ad.AppSiteAdQueues.Any(s => s.AppSite.ID == AppSiteId)) || c.CampaignBidConfigs.Any(b => b.AppSite.ID == AppSiteId && !b.IsDeleted))
                   && (!DateFrom.HasValue || c.CreationDate >= DateFrom) && (!DateTo.HasValue || c.CreationDate <= DateTo)
                    && (string.IsNullOrEmpty(Name) || c.Name.ToLower().Contains(Name.ToLower()))

         );
            return filter;
        }
        public override Func<Model.Campaign.AdGroup, bool> GetWhere()
        {
            if (Name == null)
            {
                Name = string.Empty;
            }
            Func<Model.Campaign.AdGroup, bool> filter = (c => !c.IsDeleted
                            && (string.IsNullOrEmpty(Name) || c.Name.ToLower().Contains(Name.ToLower()))
                            );
            return filter;
        }
    }
}
