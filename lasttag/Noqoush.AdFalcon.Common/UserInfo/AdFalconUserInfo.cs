using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using Noqoush.Framework.UserInfo;
using Noqoush.Framework.Security;
//using ProtoBuf.Meta;

namespace Noqoush.AdFalcon.Common.UserInfo
{
   
    [DataContract]
    //[ProtoBuf.ProtoContract()]

    public class AdFalconUserInfo : WrapperUserInfo, IUserInfo
    {
        public AdFalconUserInfo()
        {

        }
        //static AdFalconUserInfo()
        //{
           
        //    RuntimeTypeModel.Default.Add(typeof(IUserInfo), true)
        //     .AddSubType(7000, typeof(AdFalconUserInfo));
        //    ProtoBuf.Serializer.PrepareSerializer<AdFalconUserInfo>();
        //    ProtoBuf.Serializer.PrepareSerializer<IUserInfo>();
        //}
        public AdFalconUserInfo(string firstName, string lastName, int? userId, int? accountId, string userAgreementVersion, bool allowAPIAccess, bool isPrimaryUser, int[] Permissions, int AccountRole, decimal VATValue, string emailAddress, string company, bool IsReadOnly)
        {
            FirstName = firstName;
            LastName = lastName;
            UserId = userId;
            this.IsReadOnly = IsReadOnly;
            AccountId = accountId;
            OriginalAccountId = accountId;
            OriginalUserId = userId;
            UserAgreementVersion = userAgreementVersion;
            AllowAPIAccess = allowAPIAccess;
            IsPrimaryUser = isPrimaryUser;
            this.Permissions = Permissions;
            this.AccountRole = AccountRole;
            this.VATValue = VATValue;

            this.EmailAddress = emailAddress;
            if (!string.IsNullOrEmpty(company))
            {
                this.Company = company;
            }
            else
            {

                this.Company = FirstName + " " + LastName;
            }
        }
        public AdFalconUserInfo(string firstName, string lastName, int? userId, int? accountId, string userAgreementVersion, bool allowAPIAccess, bool isPrimaryUser, string emailAddress, string company, bool IsReadOnly)
        {
            this.IsReadOnly = IsReadOnly;
            FirstName = firstName;
            LastName = lastName;
            UserId = userId;
            AccountId = accountId;
            OriginalAccountId = accountId;
            OriginalUserId = userId;
            UserAgreementVersion = userAgreementVersion;
            AllowAPIAccess = allowAPIAccess;
            IsPrimaryUser = isPrimaryUser;

            this.EmailAddress = emailAddress;
            if (!string.IsNullOrEmpty(company))
            {
                this.Company = company;
            }
            else
            {

                this.Company = FirstName + " " + LastName;
            }
            // this.Permissions = Permissions;
        }
        [DataMember(Order =1)]
        //[ProtoBuf.ProtoMember(1)]
        public string FirstName { get; set; }
        [DataMember(Order = 2)]
        //[ProtoBuf.ProtoMember(2)]
        public string LastName { get; set; }
        [DataMember(Order = 3)]
        //[ProtoBuf.ProtoMember(3)]
        public int? UserId { get; set; }
        [DataMember(Order = 4)]
        //[ProtoBuf.ProtoMember(4)]
        public int? AccountId { get; set; }
        
       // [ProtoBuf.ProtoMember(5)]
        [DataMember(Order = 5)]
        public int? OriginalAccountId { get; set; }

        [DataMember(Order = 6)]
        //[ProtoBuf.ProtoMember(6)]
        public ImpersonatedAccountInfo ImpersonatedAccount { get; set; }

        [DataMember(Order = 7)]
       // [ProtoBuf.ProtoMember(7)]
        public string UserAgreementVersion { get; set; }

        [DataMember(Order = 8)]
        //[ProtoBuf.ProtoMember(8)]
        public bool AllowAPIAccess { get; set; }


        [DataMember(Order = 9)]
        //[ProtoBuf.ProtoMember(8)]
        public bool IsPrimaryUser { get; set; }


        [DataMember(Order = 10)]
        //[ProtoBuf.ProtoMember(8)]
        public int? OriginalUserId { get; set; }

        [DataMember(Order = 11)]
        //[ProtoBuf.ProtoMember(8)]
        public int[] Permissions { get; set; }
        [DataMember(Order = 12)]
        //[ProtoBuf.ProtoMember(8)]
        public int AccountRole { get; set; }



        [DataMember(Order = 13)]
        //[ProtoBuf.ProtoMember(4)]
        public decimal VATValue { get; set; }




        [DataMember(Order = 14)]
        //[ProtoBuf.ProtoMember(8)]
        public bool SwitchAccountSet { get; set; }




        [DataMember(Order = 15)]
        //[ProtoBuf.ProtoMember(8)]
        public string EmailAddress { get; set; }




        [DataMember(Order = 16)]
        //[ProtoBuf.ProtoMember(8)]
        public string Company { get; set; }


        [DataMember(Order = 17)]
        //[ProtoBuf.ProtoMember(8)]
        public int? SubUserId { get; set; }




        [DataMember(Order = 18)]
        //[ProtoBuf.ProtoMember(8)]
        public bool IsReadOnly { get; set; }



        [DataMember(Order = 19)]
        //[ProtoBuf.ProtoMember(6)]
        public string LastActionDone { get; set; }


    }
}
