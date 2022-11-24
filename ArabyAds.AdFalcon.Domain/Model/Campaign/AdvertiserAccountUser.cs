using ArabyAds.AdFalcon.Domain.Model.Account;
using ArabyAds.Framework.DomainServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArabyAds.AdFalcon.Domain.Model.Campaign
{
    public class AdvertiserAccountUser : IEntity<int>
    {
        public virtual int ID { get; protected set; }
        //public virtual string Name { get; set; }
        public virtual bool Read { get; set; }
        public virtual bool Write { get; set; }
        public virtual bool IsDeleted { get; set; }
        public virtual User User { get; set; }
       public virtual AccountInvitation Invitation { get; set; }
        public virtual AdvertiserAccount Link { get; set; }
        public virtual string GetDescription()
        {
            return string.Empty;
        }
    }


    public class AdvertiserAccountReadOnlyUser : IEntity<int>
    {
        public virtual int ID { get; protected set; }
        //public virtual string Name { get; set; }
 
        public virtual bool IsDeleted { get; set; }
        public virtual User User { get; set; }
        public virtual AccountInvitation Invitation { get; set; }
        //public virtual Account.Account Account { get; set; }
        public virtual AdvertiserAccount Link { get; set; }
        public virtual string GetDescription()
        {
            return string.Empty;
        }
    }
}
