using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using ArabyAds.AdFalcon.Domain.Common.Model.AppSite;
using ProtoBuf;

namespace ArabyAds.AdFalcon.Domain.Common.Repositories
{
    [ProtoContract]
    [ProtoInclude(100,typeof(AppSiteCriteria))]
    [ProtoInclude(101,typeof(AllAppSiteCriteria))]
    public class AppSiteCriteriaBase 
    {
        [ProtoMember(1)]
        public bool? IgnoreIsPrimaryUser { get; set; }
    
        [ProtoMember(2)]
        public int? AccountId { get; set; }
        [ProtoMember(3)]
        public bool IsPrimaryUser { get; set; }
        [ProtoMember(4)]
        public int? UserId { get; set; }
        [ProtoMember(5)]
        public int? Id { get; set; }
        [ProtoMember(6)]
        public string PublisherId { get; set; }
        [ProtoMember(7)]
        public string AccountName { get; set; }
        [ProtoMember(8)]
        public DateTime? DateFrom { get; set; }
        [ProtoMember(9)]
        public DateTime? DateTo { get; set; }
        [ProtoMember(10)]
        public int? TypeId { get; set; }
        [ProtoMember(11)]
        public int Page { get; set; }
        [ProtoMember(12)]
        public int Size { get; set; }
        [ProtoMember(13)]
        public string Name { get; set; }
        [ProtoMember(14)]
        public int? ExecludedAppId { get; set; }
        [ProtoMember(15)]
        public string Email { get; set; }
        [ProtoMember(16)]
        public int? StatusId { get; set; }

        [ProtoMember(17)]
        public string SubPublisherId { get; set; }



    }
    [ProtoContract]
    public class AppSiteCriteria : AppSiteCriteriaBase
    {

        [ProtoMember(1)]
        public string CompanyName { get; set; }
       

    }
    [ProtoContract]
    public class AllAppSiteCriteria : AppSiteCriteriaBase
    {

        [ProtoMember(1)]
        public bool  Desc { get; set; }
        [ProtoMember(2)]
        public string FieldName { get; set; }
        [ProtoMember(3)]
        public string QuickSearchExchangeNameField { get; set; }
        [ProtoMember(4)]
        public string QuickSearchField { get; set; }
        [ProtoMember(5)]
        public string CompanyName { get; set; }

        [ProtoMember(6)]
        public bool IsForSSP { get; set; }
        [ProtoMember(7)]
        public bool blockGeo  { get; set; }

        //  public string Email { get; set; }
        [ProtoMember(8)]
        public int[] AccountIds { get; set; }
        [ProtoMember(9)]
        public int  AppSiteId { get; set; }
        [ProtoMember(10)]
        public int[] ExchangeIds { get; set; }

    }
}
