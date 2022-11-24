
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
    public class AccountInvitationCriteria 
    {
        [ProtoMember(1)]
        public int id { get; set; }
        [ProtoMember(2)]
        public string invitationcode { get; set; }
        [ProtoMember(3)]
        public DateTime? DataFrom { get; set; }
        [ProtoMember(4)]
        public DateTime? DataTo { get; set; }
        [ProtoMember(5)]
        public string EmailAddress { get; set; }
        [ProtoMember(6)]
        public int accountid { get; set; }
        [ProtoMember(7)]
        public int? Page { get; set; }
        [ProtoMember(8)]
        public int Size { get; set; }

    }
}
