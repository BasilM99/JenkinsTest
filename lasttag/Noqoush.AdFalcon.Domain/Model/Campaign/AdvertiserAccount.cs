using Noqoush.AdFalcon.Domain.Common.Model.Account;
using Noqoush.AdFalcon.Domain.Model.Account;
using Noqoush.AdFalcon.Exceptions;
using Noqoush.AdFalcon.Exceptions.Core;
using Noqoush.Framework.DomainServices;
using Noqoush.Framework.UserInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Noqoush.AdFalcon.Domain.Model.Campaign
{
   public  class AdvertiserAccount : IEntity<int>
    {
        public virtual int ID { get;  set; }
        public virtual bool IsRestricted { get; set; }
        public virtual bool IsDeleted { get; set; }
        public virtual string Name { get; set; }
        public virtual Advertiser Advertiser { get; set; }

        public virtual AgencyCommission? AgencyCommission { get; set; }
        public virtual decimal  AgencyCommissionValue { get; set; }
        public virtual Account.User User { get; set; }
        public virtual Account.Account Account { get; set; }
        public virtual IList<AdvertiserAccountUser> Users { get; set; }
        public virtual IList<AdvertiserAccountReadOnlyUser> ReadOnlyUsers { get; set; }
        public virtual string GetDescription()
        {
            return Advertiser.GetDescription() +"-"+ Account.GetName();
        }
        public virtual void setAgencyCommission(AgencyCommission agC)
        {
            if (agC == Noqoush.AdFalcon.Domain.Common.Model.Account.AgencyCommission.Undefined)
            {
                this.AgencyCommission = null;
            }
            else
            {
                this.AgencyCommission = agC;
            }

        }

        public virtual AgencyCommission getAgencyCommission()
        {
            if (this.AgencyCommission.HasValue)
                return this.AgencyCommission.Value;
            else
                return Noqoush.AdFalcon.Domain.Common.Model.Account.AgencyCommission.Undefined;

        }
        public virtual void Validate(bool checkSecurity, bool statusCheck = false)
        {
            if (checkSecurity)
            {
                if ((this.Account != null) && (this.Account.ID != Framework.OperationContext.Current.UserInfo<IUserInfo>().AccountId.Value))
                {
                    throw new AccountNotValidException();
                }

                if (Framework.OperationContext.Current.UserInfo<IUserInfo>().IsReadOnly)
                {
                    var result = this.IsRestricted  && this.Users.Any(s => s.User.ID == Framework.OperationContext.Current.UserInfo<IUserInfo>().UserId.Value && s.IsDeleted == false);
                    if (!result)
                        throw new AccountNotValidException();
                }
                if (this.IsRestricted)
                {

                    if (Framework.OperationContext.Current.UserInfo<IUserInfo>().IsPrimaryUser)
                        return;
                    var result = this.Users.Any(s => s.User.ID == Framework.OperationContext.Current.UserInfo<IUserInfo>().UserId.Value && s.IsDeleted == false);
                    if(!result)
                        throw new AccountNotValidException();
                }
            }
            //if (this.IsDeleted)
            //{

            //    throw new DataNotFoundException();
            //}
            
        }

        public virtual bool Delete()
        {
            return this.IsDeleted=true;
        }
    }
}
