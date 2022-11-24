using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Noqoush.AdFalcon.Domain.Common.Model.Account;
using Noqoush.AdFalcon.Domain.Common.Model.AppSite;


namespace Noqoush.AdFalcon.Domain.Common.Repositories.Account
{
    public class UserCriteriaBase 
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
       
    }
}
