using Noqoush.AdFalcon.Domain.Common.Model.Account;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Account;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Account.Fund;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Noqoush.AdFalcon.Web.Controllers.Model.User
{
    public class AccountViewModel
    {
        public int AccountId { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string CompanyName { get; set; }
        public string Address { get; set; }
        public string DateString { get; set; }
        public string Date2String { get; set; }
        public string Email { get; set; }
        public string ApprovalNote { get; set; }
        public string StatusName { get; set; }
        public string CountryName { get; set; }
        public string Role { get; set; }
        public string CompanyTypeName { get; set; }
        public string Note { get; set; }
        public bool IsBlocked { get; set; }
        public bool IsSecondPrimaryUser { get; set; }
        
        public string PermissionCodes { get; set; }
        public decimal VATValue { get; set; }
        public UserType UserType { get; set; }
        public int UserTypeId
        {
            get
            {

                return (int)UserType;

            }
            set { }
        }
        public string UserTypeString { get {

                return UserType.ToText();

            } set { } }
        public override string ToString()
        {
            return string.Format("{0}        {1}        {2}", Name, CompanyName, Email);
        }
    }
    public class TransactionVATHistoryModel
    {
        public int StatusId { get; set; }
        public string Name { get; set; }
        public string CompanyName { get; set; }
        public int? AccountId{ get; set; }
        public IList<AccountViewModel> Users { get; set; }
        public string AccountName { get; set; }
        public string Email { get; set; }
        public long TotalCount { get; set; }
       
        public IEnumerable<FundTransactionDto> Items { get; set; }




      
       
        public bool hideCurrentUser { get; set; }
        public bool hideNonPrimary { get; set; }

    }
    public class AccountSearchViewModel
    {
        public int Role { get; set; }
        public int StatusId { get; set; }
        public string Name { get; set; }
        public string CompanyName { get; set; }
        public int? AccountIdValue { get; set; }
        public string Email { get; set; }
        public long TotalCount { get; set; }
        public IList<AccountViewModel> Users { get; set; }
        public bool hideCurrentUser { get; set; }
        public bool hideNonPrimary { get; set; }
        public bool hideAdmin { get; set; }
        public IList<Model.Action> ToolTips { get; set; }
    }

    public class AccountSearchSaveModel
    {
        public bool publisherUsers { get; set; }
        public string AccountIdStr { get; set; }
        public string Name { get; set; }
        public string CompanyName { get; set; }
        public string Email { get; set; }
        public int? AccountId { get; set; }
        public bool hideCurrentUser { get; set; }
        public bool hideNonPrimary { get; set; }
        public bool hideAdmin { get; set; }
        public int StatusId { get; set; }
        public int? UserId { get; set; }
        public string returnUrl { get; set; }

        public int RoleId { get; set; }

    }
}
