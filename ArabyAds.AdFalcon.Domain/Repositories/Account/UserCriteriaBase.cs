using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using ArabyAds.AdFalcon.Domain.Model.Account;
using ArabyAds.AdFalcon.Domain.Model.AppSite;
using ArabyAds.Framework.Persistence;

namespace ArabyAds.AdFalcon.Domain.Repositories.Account
{
    public class UserCriteriaBase : CriteriaBase<User>
    {
        public bool publisherUsers { get; set; }
        public string Name { get; set; }
        public string CompanyName { get; set; }
        public string UserName { get; set; }
        public string SubPublisherId { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public string Email { get; set; }
        public int? AccountId { get; set; }
        public int Page { get; set; }
        public int Size { get; set; }
        public bool IsBlocked { get; set; }
        public bool NonAdmin { get; set; }
        public bool hideCurrentUser { get; set; }
        public bool hideNonPrimary { get; set; }
        public bool hideAdmin { get; set; }
        public int Role { get; set; }
        public int StatusId { get; set; }
        public IList<User> StatusIdsdd { get; set; }

        public void CopyFromCommonToDomain(ArabyAds.AdFalcon.Domain.Common.Repositories.Account.UserCriteriaBase Commoncr)
        {





        UserName = Commoncr.UserName;

            CompanyName = Commoncr.CompanyName;
            Name = Commoncr.Name;


        SubPublisherId = Commoncr.SubPublisherId;

            //StatusIdsdd = Commoncr.StatusIdsdd;

            DateFrom = Commoncr.DateFrom;

            DateTo = Commoncr.DateTo;
            Email = Commoncr.Email;


            AccountId = Commoncr.AccountId;

            IsBlocked = Commoncr.IsBlocked;

            NonAdmin = Commoncr.NonAdmin;

            hideCurrentUser = Commoncr.hideCurrentUser;





        publisherUsers = Commoncr.publisherUsers;

            //StatusIdsdd = Commoncr.StatusIdsdd;

            StatusId = Commoncr.StatusId;

            Role = Commoncr.Role;
            AccountId = Commoncr.AccountId;


            Page = Commoncr.Page;

            Size = Commoncr.Size;

            hideAdmin = Commoncr.hideAdmin;

            hideNonPrimary = Commoncr.hideNonPrimary;



        }
        //  public bool? IsPublisher { get; set; }
        public override Expression<Func<User, bool>> GetExpression()
        {
            Name = string.IsNullOrEmpty(Name) ? Name : Name.Trim().Replace(" ", "");
            CompanyName = string.IsNullOrEmpty(CompanyName) ? CompanyName : CompanyName.Trim();
            Email = string.IsNullOrEmpty(Email) ? Email : Email.Trim();
            Expression<Func<User, bool>> filter = (c => (string.IsNullOrWhiteSpace(Email) || c.EmailAddress.Contains(Email)) && (string.IsNullOrWhiteSpace(CompanyName) || c.Company.Contains(CompanyName)) && (!AccountId.HasValue || c.UserAccounts.Any(M=>M.Account.ID == AccountId.Value)) && (string.IsNullOrWhiteSpace(Name) || (c.FirstName).Contains(Name) || (c.LastName).Contains(Name)));
            return filter;
        }

        public override Func<User, bool> GetWhere()
        {
            //var publisherAccountsIds = new List<int>() { 1,2};
            Name = string.IsNullOrEmpty(Name) ? Name : Name.Trim();
            CompanyName = string.IsNullOrEmpty(CompanyName) ? CompanyName : CompanyName.Trim();
            Email = string.IsNullOrEmpty(Email) ? Email : Email.Trim();

            Func<User, bool> filter = (c => (string.IsNullOrWhiteSpace(Email) || c.EmailAddress.IndexOf(Email, StringComparison.OrdinalIgnoreCase) >= 0) &&
                                            (string.IsNullOrWhiteSpace(CompanyName) || (!string.IsNullOrWhiteSpace(c.Company) && c.Company.IndexOf(CompanyName, StringComparison.OrdinalIgnoreCase) >= 0)) &&
                                            (!AccountId.HasValue || c.UserAccounts.Any(M => M.Account.ID == AccountId.Value)) &&
                                            (string.IsNullOrWhiteSpace(Name) || (c.FirstName + " " + c.LastName).IndexOf(Name, StringComparison.OrdinalIgnoreCase) >= 0));
            //   (AccountId.HasValue && publisherAccountsIds.Contains(AccountId.Value)));

            return filter;
        }
    }
}
