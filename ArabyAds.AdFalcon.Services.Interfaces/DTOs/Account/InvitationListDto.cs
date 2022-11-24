using ArabyAds.AdFalcon.Domain.Common.Model.Account;

using ArabyAds.Framework.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using ProtoBuf;
using System.Text;
using System.Threading.Tasks;

namespace ArabyAds.AdFalcon.Services.Interfaces.DTOs.Account
{
    [ProtoContract]
    public class InvitationListDto
    {
        [ProtoMember(1)]
        public IEnumerable<InvitationDto> Items { get; set; } = new List<InvitationDto>();
       [ProtoMember(2)]
        public long TotalCount { get; set; }
    }
    [ProtoContract]
    public class InvitationDto
    {
       [ProtoMember(1)]
        public int id { get; set; }
       [ProtoMember(2)]
        public string invitationcode { get; set; }
       [ProtoMember(3)]
        public DateTime InvitationDate { get; set; }
       [ProtoMember(4)]
        public string EmailAddress { get; set; }
       [ProtoMember(5)]
        public int accountid { get; set; }
       [ProtoMember(6)]
        public virtual bool IsAccepted { get; set; }

       [ProtoMember(7)]
        public virtual UserType UserType { get; set; }

       [ProtoMember(8)]
        public virtual string IsAcceptedString
        {
            get
            {
                return IsAccepted ? ResourceManager.Instance.GetResource("Yes") : ResourceManager.Instance.GetResource("No");
            }
            set { }
        }


       [ProtoMember(9)]
        public virtual string UserTypeString
        {
            get
            {
                return UserType.ToText();
            }
            set { }
        }


       [ProtoMember(10)]
        public string CompanyName { get; set; }

    }
}
