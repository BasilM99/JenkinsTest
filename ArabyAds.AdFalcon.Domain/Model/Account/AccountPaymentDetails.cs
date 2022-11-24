using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using ArabyAds.AdFalcon.Common;
using ArabyAds.AdFalcon.Domain.Common.Model.Account;
using ArabyAds.Framework.DomainServices;
using ArabyAds.Framework.ExceptionHandling.Exceptions;

namespace ArabyAds.AdFalcon.Domain.Model.Account
{
    //[DataContract(Name = "PayemntAccountType")]
    //public enum PayemntAccountType
    //{
    //    [EnumMember]
    //    [EnumText("BankAccount", "BankAccount")]
    //    Bank = 1,
    //    [EnumMember]
    //    [EnumText("PayPal", "BankAccount")]
    //    PayPal =2
    //}
    //[DataContract(Name = "PayemntAccountSubType")]
    //public enum PayemntAccountSubType
    //{
    //    [EnumMember]
    //    [EnumText("both", "Global")]
    //    Both = 1,
    //    [EnumMember]
    //    [EnumText("Payment", "Global")]
    //    Payment = 2,
    //    [EnumText("Fund", "Global")]
    //    [EnumMember]
    //    Fund = 3
    //}
    public class AccountPaymentDetails : IEntity<int>
    {
        private const string _format = "{0}:{1}";
        public virtual int ID { get; set; }
        public virtual Account Account { get; set; }
        public virtual PayemntAccountType AccountType { get; set; }
        public virtual PayemntAccountSubType SubType { get; set; }
        public virtual bool IsDeleted { get; set; }
        public virtual bool IsDefault { get; set; }
        public virtual bool IsSystem { get; set; }
        public virtual bool IsActive { get; set; }
        public virtual DateTime ActiveFrom { get; set; }
        public virtual DateTime? ActiveTo { get; set; }
        public virtual bool IsValid { get; set; }
        public virtual bool IsHasValue { get; set; }
        public  virtual void Validate(bool checkEmpty=false)
        {
            
        }
        public virtual IList<ErrorData> GetValidateErrors()
        {
            return new List<ErrorData>();
        }
        public virtual string GetDescription()
        {
            return string.Format(_format,  Framework.Resources.ResourceManager.Instance.GetResource("AccountHistory", "Titles") +":" +AccountType.ToText(),SubType.ToText());
        }

        public virtual void DeActive()
        {
            this.IsActive = false;
            this.ActiveTo = Framework.Utilities.Environment.GetServerTime();
            this.IsDefault = false;
        }
    }
}