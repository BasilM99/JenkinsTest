using System;
using System.Linq.Expressions;
using ArabyAds.Framework.Persistence;
using ArabyAds.AdFalcon.Domain.Model.Core;
using System.Collections.Generic;
using System.Linq;
using ArabyAds.Framework;

namespace ArabyAds.AdFalcon.Domain.Repositories.Campaign.Creative
{
    public class AdsCriteria : CriteriaBase<Model.Campaign.AdCreative>
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


        public void CopyFromCommonToDomain(ArabyAds.AdFalcon.Domain.Common.Repositories.Campaign.Creative.AdsCriteria Commoncr)
        {
            CampaignId = Commoncr.CampaignId;

            GroupId = Commoncr.GroupId;
            Name = Commoncr.Name;
            StatusId = Commoncr.StatusId;
            Permissions = Commoncr.Permissions;
            DataFrom = Commoncr.DataFrom; DataTo = Commoncr.DataTo;

            Page = Commoncr.Page;
            Size = Commoncr.Size;


        }
        public override Expression<Func<Model.Campaign.AdCreative, bool>> GetExpression()
        {
            Expression<Func<Model.Campaign.AdCreative, bool>> filter = (c => !c.IsDeleted
            && (!StatusId.HasValue || c.Status.ID == StatusId)
            && (string.IsNullOrEmpty(Name) || c.Name.ToLower().Contains(Name.ToLower()))
            );

            return filter;
        }
        public override Func<Model.Campaign.AdCreative, bool> GetWhere()
        {
            Func<Model.Campaign.AdCreative, bool> filter = (c => !c.IsDeleted
            && (!StatusId.HasValue || c.Status.ID == StatusId)
            && (string.IsNullOrEmpty(Name) || c.Name.ToLower().Contains(Name.ToLower()))
            && c.Parent == null
            );

            return filter;
        }
    }

    public class AdsSummaryCriteria : CriteriaBase<Model.Campaign.AdCreative>
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


        public void CopyFromCommonToDomain(ArabyAds.AdFalcon.Domain.Common.Repositories.Campaign.Creative.AdsSummaryCriteria Commoncr)
        {
            CampaignId = Commoncr.CampaignId;

            GroupId = Commoncr.GroupId;
            Account = Commoncr.Account;
            StatusId = Commoncr.StatusId;
            CompanyName = Commoncr.CompanyName;
            CampaignName = Commoncr.CampaignName;

            AccountName = Commoncr.AccountName;
            DateFrom = Commoncr.DateFrom; DateTo = Commoncr.DateTo;

            Page = Commoncr.Page;
            Size = Commoncr.Size;


        }

        public override Expression<Func<Model.Campaign.AdCreative, bool>> GetExpression()
        {
            Expression<Func<Model.Campaign.AdCreative, bool>> filter = (
                c => c.IsDeleted == false && c.Parent == null &&
                    (!StatusId.HasValue || c.Status.ID == StatusId) && (c.Group.Campaign.Account.Tenant.ID == ApplicationContext.Instance.Tenant.ID) &&
                    (!DateFrom.HasValue || c.Group.Campaign.StartDate >= DateFrom) &&
                    (!DateTo.HasValue || c.Group.Campaign.StartDate <= DateTo) &&
                   (CampaignId==0 || c.Group.Campaign.ID.Equals(CampaignId)) &&
                  (GroupId==0 || c.Group.ID.Equals(GroupId)) &&

                    (string.IsNullOrWhiteSpace(CampaignName) || c.Group.Campaign.Name.Contains(CampaignName)) &&
                    (string.IsNullOrWhiteSpace(CompanyName) || c.Group.Campaign.Account.PrimaryUser.Company.Contains(CompanyName)) &&
                    //(string.IsNullOrWhiteSpace(AccountName) || (c.Group.Campaign.Account.PrimaryUser.FirstName+" "+c.Group.Campaign.Account.PrimaryUser.LastName).Contains(AccountName))
                    (string.IsNullOrWhiteSpace(AccountName) || c.Group.Campaign.Account.PrimaryUser.FirstName.Contains(AccountName) || c.Group.Campaign.Account.PrimaryUser.LastName.Contains(AccountName))
                );
            return filter;
        }
        public override Func<Model.Campaign.AdCreative, bool> GetWhere()
        {
            Func<Model.Campaign.AdCreative, bool> filter = (c => c.IsDeleted == false && (!StatusId.HasValue || c.Status.ID == StatusId));
            return filter;
        }
    }
}
