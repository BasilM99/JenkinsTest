
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Noqoush.AdFalcon.Business.Domain.Exceptions;
using Noqoush.AdFalcon.Domain.Model.AppSite.Filtering;
using Noqoush.AdFalcon.Domain.Model.Campaign;
using Noqoush.AdFalcon.Domain.Model.Core;
using Noqoush.AdFalcon.Exceptions;
using Noqoush.AdFalcon.Exceptions.Core;
using Noqoush.Framework;
using Noqoush.Framework.DomainServices;
using Noqoush.Framework.Security;
using Noqoush.Framework.DataAnnotations;
using System.Runtime.Serialization;
using Noqoush.AdFalcon.Domain.Repositories;
using Noqoush.AdFalcon.Domain.Repositories.Core;
using Noqoush.Framework.UserInfo;
using Noqoush.Framework.Utilities;
using Noqoush.AdFalcon.Domain.Common.Model.Account.PMP;

namespace Noqoush.AdFalcon.Domain.Model.Account.PMP
{

    ////[DataContract(Name = "DealType")]
    ////public enum DealType
    ////{
    ////    [EnumMember]

    ////    Undefined = 0,
    ////    [EnumMember]
    ////    [EnumText("PrivateAuction", "PMPDeal")]
    ////    PrivateAuction = 1,

    ////    [EnumMember]
    ////    [EnumText("Fixed", "PMPDeal")]
    ////    Fixed = 2
    ////}


    ////[DataContract(Name = "AdGroupTargetingDealType ")]
    ////public enum AdGroupTargetingDealType
    ////{
    ////    [EnumMember]

    ////    Undefined = 0,

    ////    [EnumMember]
    ////    [EnumText("PMP", "PMPDeal")]
    ////    PMP = 1,
    ////    [EnumMember]
    ////    [EnumText("OpenInventorytargeting", "PMPDeal")]
    ////    OpenInventorytargeting = 2


    ////}





    public class PMPDeal : IEntity<int>
    {
        private IAccountRepository _accountRepository = IoC.Instance.Resolve<IAccountRepository>();
        private ISSPPartnerRepository _sSPPartnerRepository = IoC.Instance.Resolve<ISSPPartnerRepository>();

        public virtual bool IsDeleted
        {
            get;
            set;
        }
        public virtual bool IsGlobal
        {
            get;
            set;
        }
        
        public virtual int ID
        {
            get;
            set;
        }
        public virtual string Note
        {
            get;
            set;
        }

        public virtual string DealID
        {
            get;
            set;
        }
        public virtual string PublisherName
        {
            get;
            set;
        }
        public virtual string Name
        {
            get;
            set;
        }

        public virtual string Description
        {
            get;
            set;
        }
        public virtual long UniqueId
        {
            get;
            set;
        }
        public virtual DealType Type
        {
            get;
            set;
        }
        public virtual AdvertiserAccount AdvertiserAccount { get; set; }
        public virtual DateTime CreationDate { get; set; }
        public virtual DateTime? EndDate { get; set; }
        public virtual DateTime? StartDate { get; set; }
        public virtual decimal? Price { get; set; }
        //public virtual string DealId
        //{
        //    get;
        //    set;
        //}

        public virtual User User { get; set; }
        public virtual Advertiser Advertiser { get; set; }
        public virtual Account Account { get; set; }

        public virtual Account Publisher { get; set; }

        public virtual SSPPartner Exchange { get; set; }
        public virtual string GetExchangeDes(string id)
        {

            return _sSPPartnerRepository.Get(Convert.ToInt32(id)).Name;

        }
        public virtual string GetPublisherDes(string id)
        {

            return _accountRepository.Get(Convert.ToInt32(id)).GetName();

        }


        public virtual void Delete()
        {

            this.IsDeleted = true;
        }

        public virtual string GetDescription()
        {
            return this.ID.ToString();
        }
        public virtual IList<PMPDealTargeting> Targetings
        {
            get;
            set;
        }
        public virtual void RemoveTargeting(PMPDealTargeting targetingBase)
        {
            var targetings = this.Targetings;
            var targetingObj = targetings.FirstOrDefault(targeting => targeting.ID == targetingBase.ID);
            if (targetingObj != null)
            {
                targetings.Remove(targetingBase);
            }
            else
            {
                //this should not happen
                //do nothing
                //TODO:throw exception
            }
        }
        public virtual void AddTargeting(PMPDealTargeting targetingBase)
        {
            var targetings = this.Targetings;
            if (targetingBase.ID == 0)
            {
                targetingBase.Deal = this;
                targetings.Add(targetingBase);
            }
            else
            {
                var targetingObj = targetings.FirstOrDefault(targeting => targeting.ID == targetingBase.ID);
                if (targetingObj == null)
                {
                    targetings.Add(targetingBase);
                }
                else
                {
                    //this should not happen
                    //do nothing
                    //TODO:throw exception
                }
            }
        }



        public virtual void Validate(bool checkSecurity, bool statusCheck = false, bool validateDates = false)
        {
            bool IsValid = false;
            if (checkSecurity)
            {
                if ((this.Account != null) && (this.Account.ID != OperationContext.Current.UserInfo<IUserInfo>().AccountId.Value))
                {
                    throw new AccountNotValidException();
                }

                /*
                if (!Framework.OperationContext.Current.UserInfo<Noqoush.Framework.UserInfo.IUserInfo>().IsPrimaryUser)
                {
                    if ((this.User != null) && (this.User.ID != Framework.OperationContext.Current.UserInfo<Noqoush.Framework.UserInfo.IUserInfo>().UserId))
                    {
                        throw new AccountNotValidException();
                    }
                }*/
            }

            if (this.AdvertiserAccount != null)
                this.AdvertiserAccount.Validate(checkSecurity);
            /*if (this.IsDeleted)
            {

                throw new DataNotFoundException();
            }*/

          
            IsValid = true;
        }



    }
}
