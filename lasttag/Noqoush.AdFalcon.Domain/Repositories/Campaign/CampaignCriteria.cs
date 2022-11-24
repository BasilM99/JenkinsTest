using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Noqoush.AdFalcon.Domain.Common.Model.Campaign;
using Noqoush.AdFalcon.Domain.Model.AppSite;
using Noqoush.AdFalcon.Domain.Model.Campaign;
using Noqoush.Framework.Persistence;

namespace Noqoush.AdFalcon.Domain.Repositories.Campaign
{
    public class CampaignCriteria : CriteriaBase<Model.Campaign.Campaign>
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




        public void CopyFromCommonToDomain(Noqoush.AdFalcon.Domain.Common.Repositories.Campaign.CampaignCriteria Commoncr)
        {
            AccountId = Commoncr.AccountId;
            userId = Commoncr.userId;
            IsPrimaryUser = Commoncr.IsPrimaryUser;
            AdvertiserAccountId = Commoncr.AdvertiserAccountId;

            Name = Commoncr.Name;
            AdvertiserId = Commoncr.AdvertiserId;
            AppSiteId = Commoncr.AppSiteId;

            IsPrimaryUser = Commoncr.IsPrimaryUser;
        
            ActiveCampaigns = Commoncr.ActiveCampaigns;

            DataCreate = Commoncr.DataCreate;


            DataFrom = Commoncr.DataFrom; DataTo = Commoncr.DataTo;

            Page = Commoncr.Page;
            Size = Commoncr.Size;
            CampaignType = Commoncr.CampaignType;
            OtherCampaignType = Commoncr.OtherCampaignType;



        }



        public override Expression<Func<Model.Campaign.Campaign, bool>> GetExpression()
        {

            if (Name==null)
            {
                Name = string.Empty;
            }
            Expression<Func<Model.Campaign.Campaign, bool>> filter =
                (c => c.IsDeleted == false
                && c.Account.ID == AccountId /*&&(!userId.HasValue || c.User.ID==userId)*/

                && (!AdvertiserId.HasValue || c.Advertiser.ID == AdvertiserId)
                   && (!AdvertiserAccountId.HasValue || c.AdvertiserAccount.ID == AdvertiserAccountId)
                 && (!DataCreate.HasValue || c.CreationDate>= DataCreate)
                && (c.CampaignType == CampaignType || c.CampaignType == OtherCampaignType)
                && (!AppSiteId.HasValue || c.AdGroups.Any(a => a.Ads.Any(ad => ad.AppSiteAdQueues.Any(s => s.AppSite.ID == AppSiteId))))
                && (string.IsNullOrEmpty(Name) || c.Name.ToLower().Contains(Name.ToLower()))
                );
            return filter;
        }

        public override Func<Model.Campaign.Campaign, bool> GetWhere()
        {
            // Func<Model.Campaign.Campaign, bool> filter = (c => c.IsDeleted == false && (!StatusId.HasValue || c.Status.ID == StatusId) && c.Account.ID == AccountId);
            Func<Model.Campaign.Campaign, bool> filter = (c => c.IsDeleted == false && c.Account.ID == AccountId /*&&(!userId.HasValue || c.User.ID==userId)*/ && (!AdvertiserId.HasValue || c.Advertiser.ID == AdvertiserId) && (!AdvertiserAccountId.HasValue || c.AdvertiserAccount.ID == AdvertiserAccountId) && (!DataCreate.HasValue || c.CreationDate>= DataCreate)   && (c.CampaignType == CampaignType || c.CampaignType==OtherCampaignType));
            return filter;
        }
    }

    public class AllCampaignCriteria : CriteriaBase<Model.Campaign.Campaign>
    {
        public int? AppSiteId { get; set; }
        public AllCampaignCriteria()
        {
        }

        public void CopyFromCommonToDomain(Noqoush.AdFalcon.Domain.Common.Repositories.Campaign.AllCampaignCriteria Commoncr)
        {
            AppSiteId = Commoncr.AppSiteId;
         


        }
        public override Expression<Func<Model.Campaign.Campaign, bool>> GetExpression()
        {
            Expression<Func<Model.Campaign.Campaign, bool>> filter =
                (c => c.IsDeleted == false
                && (!AppSiteId.HasValue || c.AdGroups.Any(a => a.Ads.Any(ad => ad.AppSiteAdQueues.Any(s => s.AppSite.ID == AppSiteId))))
                );
            return filter;
        }

        public override Func<Model.Campaign.Campaign, bool> GetWhere()
        {
            // Func<Model.Campaign.Campaign, bool> filter = (c => c.IsDeleted == false && (!StatusId.HasValue || c.Status.ID == StatusId) && c.Account.ID == AccountId);
            Func<Model.Campaign.Campaign, bool> filter = (c => c.IsDeleted == false && (!AppSiteId.HasValue || c.AdGroups.Any(a => a.Ads.Any(ad => ad.AppSiteAdQueues.Any(s => s.AppSite.ID == AppSiteId)))));
            return filter;
        }
    }
}
