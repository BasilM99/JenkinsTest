using ArabyAds.AdFalcon.Domain.Common.Model.Account;
using ArabyAds.Framework.DomainServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArabyAds.AdFalcon.Domain.Model.Account
{
    public class AccountInvitation : IEntity<int>
    {
        public virtual bool IsDeleted
        {
            get;
            set;
        }
        public virtual string InvitationCode { get; set; }
        public virtual string EmailAddress { get; set; }
        public virtual int ID { get;  set; }
        public virtual Account Account { get; set; }
        public virtual DateTime InvitationDate { get; set; }
        public virtual bool IsAccepted { get; set; }

        public virtual UserType UserType { get; set; }
        public virtual string GetDescription()
        {

            return EmailAddress;


        }
    }
}
