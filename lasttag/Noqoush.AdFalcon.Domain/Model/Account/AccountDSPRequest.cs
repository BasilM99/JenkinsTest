
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Noqoush.AdFalcon.Domain.Model.Account.Discount;
using Noqoush.AdFalcon.Domain.Model.Campaign;
using Noqoush.AdFalcon.Domain.Model.Core;
using Noqoush.Framework.ExceptionHandling.Exceptions;
using Noqoush.Framework.Resources;
using Noqoush.Framework.DomainServices;

using System.Security.Cryptography;
using System.Text;
using Noqoush.AdFalcon.Domain.Common.Model.Account;

namespace Noqoush.AdFalcon.Domain.Model.Account
{

    //[DataContract(Name = "AccountDSPRequestStatus")]
    //public enum AccountDSPRequestStatus
    //{
    //    [EnumMember]
    //    [EnumText("Undefined", "AccountDSPRequest")]
    //    Undefined =0,
    //    [EnumMember]
    //    [EnumText("New", "AccountDSPRequest")]
    //    New = 1,
    //    [EnumMember]
    //    [EnumText("Ignored", "AccountDSPRequest")]
    //    Ignored = 2,
    //    [EnumMember]
    //    [EnumText("Approved", "AccountDSPRequest")]
    //    Approved =3
    //}
    public class AccountDSPRequest : IEntity<int>
    {
        public virtual int ID { get; set; }
        public virtual bool IsDeleted { get; set; }
        public virtual string FirstName { get; set; }

        public virtual string EmailAddress { get; set; }
      //  public virtual string ApprovalNote { get; set; }

        public virtual string RequestCode { get; set; }
        public virtual string LastName { get; set; }

        public virtual string Phone { get; set; }
        public virtual DateTime RequestDate { get; set; }
        public virtual DateTime ActionDate { get; set; }
        public virtual AccountDSPRequestStatus Status {get;set;}
       // public virtual DateTime? ApprovedDate { get; set; }
        public virtual Noqoush.AdFalcon.Domain.Model.Account.Account Approver { get; set; }

        public virtual Noqoush.AdFalcon.Domain.Model.Account.Account Account { get; set; }
        public virtual CompanyType CompanyType { get; set; }
        public virtual string Company { get; set; }
        public virtual bool IsAllowNotifications { get; set; }
        public virtual string Note { get; set; }
        public virtual string ActionNote { get; set; }
        public virtual string Address1 { get; set; }
     

        public virtual Country Country { get; set; }
        public virtual string GetDescription()
        {
            return this.FirstName + " " + this.LastName;
        }
        public virtual void SetRequestCode()
        {
            RequestCode = MD5Encryption(Guid.NewGuid().ToString());

        }
        private string MD5Encryption(string originalText)
        {
           
            byte[] bs = Encoding.UTF8.GetBytes(originalText);
            using (MD5 md5 = MD5.Create())
            {
                bs = md5.ComputeHash(bs);
            }
            return BitConverter.ToString(bs).Replace("-", "");
        }
    }
}
