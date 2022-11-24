using Noqoush.AdFalcon.Domain.Common.Model.Account;

using Noqoush.Framework.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Noqoush.AdFalcon.Services.Interfaces.DTOs.Account
{
    [DataContract]
    public class InvitationListDto
    {
        [DataMember]
        public IEnumerable<InvitationDto> Items { get; set; }
        [DataMember]
        public long TotalCount { get; set; }
    }
    [DataContract]
    public class InvitationDto
    {
        [DataMember]
        public int id { get; set; }
        [DataMember]
        public string invitationcode { get; set; }
        [DataMember]
        public DateTime InvitationDate { get; set; }
        [DataMember]
        public string EmailAddress { get; set; }
        [DataMember]
        public int accountid { get; set; }
        [DataMember]
        public virtual bool IsAccepted { get; set; }

        [DataMember]
        public virtual UserType UserType { get; set; }

        [DataMember]
        public virtual string IsAcceptedString
        {
            get
            {
                return IsAccepted ? ResourceManager.Instance.GetResource("Yes") : ResourceManager.Instance.GetResource("No");
            }
            set { }
        }


        [DataMember]
        public virtual string UserTypeString
        {
            get
            {
                return UserType.ToText();
            }
            set { }
        }


        [DataMember]
        public string CompanyName { get; set; }

    }
}
