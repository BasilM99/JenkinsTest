using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using ArabyAds.AdFalcon.Domain.Common.Model.Account;
using ArabyAds.AdFalcon.Domain.Common.Model.AppSite;
using ProtoBuf;

namespace ArabyAds.AdFalcon.Domain.Common.Repositories.Account
{
    [ProtoContract]
    public class UserCriteriaBase 
    {
        [ProtoMember(1)]
        public bool publisherUsers { get; set; }
        [ProtoMember(2)]
        public string Name { get; set; }
        [ProtoMember(3)]
        public string CompanyName { get; set; }
        [ProtoMember(4)]
        public string UserName { get; set; }
        [ProtoMember(5)]
        public string SubPublisherId { get; set; }
        [ProtoMember(6)]
        public DateTime? DateFrom { get; set; }
        [ProtoMember(7)]
        public DateTime? DateTo { get; set; }
        [ProtoMember(8)]
        public string Email { get; set; }
        [ProtoMember(9)]
        public int? AccountId { get; set; }
        [ProtoMember(10)]
        public int Page { get; set; }
        [ProtoMember(11)]
        public int Size { get; set; }
        [ProtoMember(12)]
        public bool IsBlocked { get; set; }
        [ProtoMember(13)]
        public bool NonAdmin { get; set; }
        [ProtoMember(14)]
        public bool hideCurrentUser { get; set; }
        [ProtoMember(15)]
        public bool hideNonPrimary { get; set; }
        [ProtoMember(16)]
        public bool hideAdmin { get; set; }
        [ProtoMember(17)]
        public int Role { get; set; }
        [ProtoMember(18)]
        public int StatusId { get; set; }

    }
}
