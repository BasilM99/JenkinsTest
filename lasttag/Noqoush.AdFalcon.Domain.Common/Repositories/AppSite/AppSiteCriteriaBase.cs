using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Noqoush.AdFalcon.Domain.Common.Model.AppSite;



namespace Noqoush.AdFalcon.Domain.Common.Repositories
{
    public class AppSiteCriteriaBase 
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


        
    }

    public class AppSiteCriteria : AppSiteCriteriaBase
    {

        public string CompanyName { get; set; }
        public int? StatusId { get; set; }
        public string Email { get; set; }

       
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
     
    }
}
