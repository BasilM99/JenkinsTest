
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ArabyAds.AdFalcon.Domain.Common.Repositories.Account
{
    [ProtoContract]
    public class AuditTrialCriteria
    {
        [ProtoMember(1)]
        public int? AccountId { get; set; }
        [ProtoMember(2)]
        public int? UserId { get; set; }

        [ProtoMember(3)]
        public bool IsPrimaryUser { get; set; }
        [ProtoMember(4)]
        public DateTime? DataFrom { get; set; }
        [ProtoMember(5)]
        public DateTime? DataTo { get; set; }

        [ProtoMember(6)]
        public int? Page { get; set; }
        [ProtoMember(7)]
        public int Size { get; set; }
        [ProtoMember(8)]
        public string Name { get; set; }

        [ProtoMember(9)]
        public string UserName { get; set; }
        [ProtoMember(10)]
        public int Type { get; set; }
        [ProtoMember(11)]
        public int ObjectRootId { get; set; }
        // public int ObjectRootId { get; set; }
    }
}
