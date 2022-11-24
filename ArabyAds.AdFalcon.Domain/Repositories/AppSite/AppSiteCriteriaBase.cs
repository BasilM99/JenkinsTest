using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using ArabyAds.AdFalcon.Domain.Model.AppSite;
using ArabyAds.Framework;
using ArabyAds.Framework.Persistence;


namespace ArabyAds.AdFalcon.Domain.Repositories
{
    public class AppSiteCriteriaBase : CriteriaBase<AppSite>
    {
        public bool? IgnoreIsPrimaryUser { get; set; }
    
        public int? AccountId { get; set; }
        public bool IsPrimaryUser { get; set; }
        public int? UserId { get; set; }
        public int? Id { get; set; }
        public string PublisherId { get; set; }
        public string AccountName { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public int? TypeId { get; set; }
        public int Page { get; set; }
        public int Size { get; set; }
        public string Name { get; set; }
        public int? ExecludedAppId { get; set; }
        public string Email { get; set; }
        public int? StatusId { get; set; }

        public string SubPublisherId { get; set; }

        public void CopyFromCommonToDomain(ArabyAds.AdFalcon.Domain.Common.Repositories.AppSiteCriteriaBase Commoncr)
        {
            AccountId = Commoncr.AccountId;

            UserId = Commoncr.UserId;

            Page = Commoncr.Page;

            Size = Commoncr.Size;

            IsPrimaryUser = Commoncr.IsPrimaryUser;


            DateFrom = Commoncr.DateFrom;
            DateTo = Commoncr.DateTo;

            AccountName = Commoncr.AccountName;
            TypeId = Commoncr.TypeId;
                        Name = Commoncr.Name;

            SubPublisherId = Commoncr.SubPublisherId;

            Email = Commoncr.Email;

            ExecludedAppId = Commoncr.ExecludedAppId;

            StatusId = Commoncr.StatusId;



            IgnoreIsPrimaryUser = Commoncr.IgnoreIsPrimaryUser;

            AccountId = Commoncr.AccountId;

            PublisherId = Commoncr.PublisherId;

            UserId = Commoncr.UserId;

            Id = Commoncr.Id;

    }
        public override Expression<Func<AppSite, bool>> GetExpression()
        {
            Expression<Func<AppSite, bool>> filter = (c => c.IsDeleted == false && (!AccountId.HasValue || c.Account.ID == AccountId ) /*&& (!UserId.HasValue || c.User.ID == UserId)*/
                                                          && (string.IsNullOrWhiteSpace(SubPublisherId) || c.SubAppsites.Any(v => v.SubPublisherId == SubPublisherId))
                                                          && (string.IsNullOrWhiteSpace(Email) || c.Account.PrimaryUser.EmailAddress == Email) && c.ID != (this.ExecludedAppId.HasValue ? this.ExecludedAppId.Value : int.MinValue)
                                                          && (string.IsNullOrWhiteSpace(Name) || c.Name.Contains(Name)) && (!DateFrom.HasValue || c.RegistrationDate.Date >= DateFrom)
                                                          && (!DateTo.HasValue || c.RegistrationDate.Date <= DateTo)
                                                          && (string.IsNullOrWhiteSpace(AccountName) || c.Account.PrimaryUser.FirstName.Contains(AccountName) || c.Account.PrimaryUser.LastName.Contains(AccountName))
                //    && (!TypeId.HasValue) || (TypeId.Value == c.Type.ID)
                                                          );


            if (TypeId.HasValue)
            {
                filter = (c => c.IsDeleted == false && (!AccountId.HasValue || c.Account.ID == AccountId) /*&& (!UserId.HasValue || c.User.ID == UserId)*/
                                                          && (string.IsNullOrWhiteSpace(SubPublisherId) || c.SubAppsites.Any(v => v.SubPublisherId == SubPublisherId))
                                                          && (string.IsNullOrWhiteSpace(Email) || c.Account.PrimaryUser.EmailAddress == Email) && c.ID != (this.ExecludedAppId.HasValue ? this.ExecludedAppId.Value : int.MinValue)
                                                          && (string.IsNullOrWhiteSpace(Name) || c.Name.Contains(Name)) && (!DateFrom.HasValue || c.RegistrationDate.Date >= DateFrom)
                                                          && (!DateTo.HasValue || c.RegistrationDate.Date <= DateTo)
                                                          && (string.IsNullOrWhiteSpace(AccountName) || c.Account.PrimaryUser.FirstName.Contains(AccountName) || c.Account.PrimaryUser.LastName.Contains(AccountName))
                                                          && (TypeId.Value == c.Type.ID)
                                                          );
            }


            return filter;
        }

        public override Func<AppSite, bool> GetWhere()
        {
            Expression<Func<AppSite, bool>> filter = GetExpression();
            return filter.Compile();
        }
    }

    public class AppSiteCriteria : AppSiteCriteriaBase
    {

        public string CompanyName { get; set; }
        public int? StatusId { get; set; }
        public string Email { get; set; }



        public void CopyFromCommonToDomain(ArabyAds.AdFalcon.Domain.Common.Repositories.AppSiteCriteria Commoncr)
        {
            CompanyName = Commoncr.CompanyName;

            AccountId = Commoncr.AccountId;

            UserId = Commoncr.UserId;

            Page = Commoncr.Page;

            Size = Commoncr.Size;

            IsPrimaryUser = Commoncr.IsPrimaryUser;


            DateFrom = Commoncr.DateFrom;
            DateTo = Commoncr.DateTo;

            AccountName = Commoncr.AccountName;
            TypeId = Commoncr.TypeId;
            Name = Commoncr.Name;

            SubPublisherId = Commoncr.SubPublisherId;

            Email = Commoncr.Email;

            ExecludedAppId = Commoncr.ExecludedAppId;

            StatusId = Commoncr.StatusId;



            IgnoreIsPrimaryUser = Commoncr.IgnoreIsPrimaryUser;

            AccountId = Commoncr.AccountId;

            PublisherId = Commoncr.PublisherId;

            UserId = Commoncr.UserId;

            Id = Commoncr.Id;

        }

        public override Expression<Func<AppSite, bool>> GetExpression()
        {
            Expression<Func<AppSite, bool>> filter = c => c.IsDeleted == false
                                                          && (!AccountId.HasValue || c.Account.ID == AccountId)&&(c.Account.Tenant.ID == ApplicationContext.Instance.Tenant.ID) &&  /*&& (!UserId.HasValue || c.User.ID == UserId)*/
                                                          (!TypeId.HasValue || c.Type.ID == TypeId)
                                                          && (string.IsNullOrWhiteSpace(PublisherId) || c.PublisherId.Contains(Name) || string.IsNullOrWhiteSpace(Name) || c.Name.Contains(Name))

                                                          && (!DateFrom.HasValue || c.RegistrationDate.Date >= DateFrom.Value)
                                                          && (!DateTo.HasValue || c.RegistrationDate.Date <= DateTo.Value)
                                                          && (!StatusId.HasValue || c.Status.ID == StatusId)

                                                          && (string.IsNullOrWhiteSpace(AccountName) || c.Account.PrimaryUser.FirstName.Contains(AccountName) || c.Account.PrimaryUser.LastName.Contains(AccountName))
                                                          && (string.IsNullOrWhiteSpace(CompanyName) || c.Account.PrimaryUser.Company.Contains(CompanyName))
                                                            && (string.IsNullOrWhiteSpace(Email) || c.Account.PrimaryUser.EmailAddress.Contains(Email));




            return filter;
        }

        public override Func<AppSite, bool> GetWhere()
        {
            Expression<Func<AppSite, bool>> filter = GetExpression();
            return filter.Compile();
        }
    }
    public class AllAppSiteCriteria : AppSiteCriteriaBase
    {
        public bool  Desc { get; set; }
        public string FieldName { get; set; }
        public string QuickSearchExchangeNameField { get; set; }
        public string QuickSearchField { get; set; }
        public string CompanyName { get; set; }

        public bool IsForSSP { get; set; }
        public bool blockGeo  { get; set; }

        //  public string Email { get; set; }
        public int[] AccountIds { get; set; }
        public int  AppSiteId { get; set; }
        public int[] ExchangeIds { get; set; }

        public void CopyFromCommonToDomain(ArabyAds.AdFalcon.Domain.Common.Repositories.AllAppSiteCriteria Commoncr)
        {


       



        QuickSearchExchangeNameField = Commoncr.QuickSearchExchangeNameField;

            QuickSearchField = Commoncr.QuickSearchField;

            FieldName = Commoncr.FieldName;
               Desc = Commoncr.Desc;



        CompanyName = Commoncr.CompanyName;

            IsForSSP = Commoncr.IsForSSP;

            blockGeo = Commoncr.blockGeo;

            AccountIds = Commoncr.AccountIds;

            AppSiteId = Commoncr.AppSiteId;

            ExchangeIds = Commoncr.ExchangeIds;







        CompanyName = Commoncr.CompanyName;

            AccountId = Commoncr.AccountId;

            UserId = Commoncr.UserId;

            Page = Commoncr.Page;

            Size = Commoncr.Size;

            IsPrimaryUser = Commoncr.IsPrimaryUser;


            DateFrom = Commoncr.DateFrom;
            DateTo = Commoncr.DateTo;

            AccountName = Commoncr.AccountName;
            TypeId = Commoncr.TypeId;
            Name = Commoncr.Name;

            SubPublisherId = Commoncr.SubPublisherId;

            Email = Commoncr.Email;

            ExecludedAppId = Commoncr.ExecludedAppId;

            StatusId = Commoncr.StatusId;



            IgnoreIsPrimaryUser = Commoncr.IgnoreIsPrimaryUser;

            AccountId = Commoncr.AccountId;

            PublisherId = Commoncr.PublisherId;

            UserId = Commoncr.UserId;

            Id = Commoncr.Id;

        }


        public override Expression<Func<AppSite, bool>> GetExpression()
        {

            if (StatusId == null)
                StatusId = AppSiteStatus.Active.ID;

            Expression<Func<AppSite, bool>> filter = (c => c.IsDeleted == false && c.Status.ID == StatusId
                && (!AccountId.HasValue || c.Account.ID == AccountId) && (c.Account.Tenant.ID == ApplicationContext.Instance.Tenant.ID) /*&& (!UserId.HasValue || c.User.ID == UserId)*/
                && (string.IsNullOrWhiteSpace(SubPublisherId) || c.SubAppsites.Any(v => v.SubPublisherId == SubPublisherId)) //.Where(x => x.SubPublisherId == SubPublisherId).Count() > 0)
                && (string.IsNullOrWhiteSpace(Email) || c.Account.PrimaryUser.EmailAddress == Email)
                && c.ID != (this.ExecludedAppId.HasValue ? this.ExecludedAppId.Value : int.MinValue)
                && (string.IsNullOrWhiteSpace(Name) || (c.Name.Contains(Name)))
                && (!DateFrom.HasValue || c.RegistrationDate.Date >= DateFrom)
                && (!DateTo.HasValue || c.RegistrationDate.Date <= DateTo)
                &&
                (
                      (string.IsNullOrWhiteSpace(AccountName)
                      || (c.Account.PrimaryUser.FirstName.Contains(AccountName) || c.Account.PrimaryUser.LastName.Contains(AccountName) || c.Account.PrimaryUser.Company.Contains(CompanyName)))

                 )
            );

            if (TypeId.HasValue)
            {
                filter = (c => c.IsDeleted == false && c.Status.ID == StatusId
                && (!AccountId.HasValue || c.Account.ID == AccountId) && (c.Account.Tenant.ID == ApplicationContext.Instance.Tenant.ID) /*&& (!UserId.HasValue || c.User.ID == UserId)*/
                && (string.IsNullOrWhiteSpace(SubPublisherId) || c.SubAppsites.Any(v => v.SubPublisherId == SubPublisherId)) //.Where(x => x.SubPublisherId == SubPublisherId).Count() > 0)
                && (string.IsNullOrWhiteSpace(Email) || c.Account.PrimaryUser.EmailAddress == Email)
                && c.ID != (this.ExecludedAppId.HasValue ? this.ExecludedAppId.Value : int.MinValue)
                && (string.IsNullOrWhiteSpace(Name) || (c.Name.Contains(Name)))
                && (!DateFrom.HasValue || c.RegistrationDate.Date >= DateFrom)
                && (!DateTo.HasValue || c.RegistrationDate.Date <= DateTo)
                && (c.Type.ID == TypeId)
                &&
                (
                      (string.IsNullOrWhiteSpace(AccountName) || c.Account.PrimaryUser.FirstName.Contains(AccountName) || c.Account.PrimaryUser.LastName.Contains(AccountName))
                      || (string.IsNullOrWhiteSpace(CompanyName) || c.Account.PrimaryUser.Company.Contains(CompanyName))
                 )
                  );
            }
            return filter;
        }

        public override Func<AppSite, bool> GetWhere()
        {
            Expression<Func<AppSite, bool>> filter = GetExpression();
            return filter.Compile();
        }
    }
}
