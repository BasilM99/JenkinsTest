using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProtoBuf;
using ArabyAds.Framework.DataAnnotations;
using ArabyAds.AdFalcon.Domain.Common.Model.Core;
using ArabyAds.AdFalcon.Domain.Common.Model.Account;

namespace ArabyAds.AdFalcon.Services.Interfaces.DTOs.Account
{
    [ProtoContract]
    public class UserDto
    {
       [ProtoMember(1)]
        public UserType UserType { get; set; }
       [ProtoMember(2)]
        public bool IsSecondPrimaryUser { get; set; }
       [ProtoMember(3)]
        public bool AlreadyReg { get; set; }
       [ProtoMember(4)]
        public decimal VATValue { get; set; }
       [ProtoMember(5)]
        public int AccountRole { get; set; }
       [ProtoMember(6)]
        public bool ExchangeChecked { get; set; }
       [ProtoMember(7)]
        public int Id { get; set; }

       [ProtoMember(8)]
        public int AppSiteId { get; set; }
       [ProtoMember(9)]
        public string buyerCode { get; set; }
       [ProtoMember(10)]
        public int? buyerId { get; set; }

       [ProtoMember(11)]
        public bool Block { get; set; }
       [ProtoMember(12)]
        public int AccountId { get; set; }
       [ProtoMember(13)]
        public bool  Checked { get; set; }
       [ProtoMember(14)]
        public string AccountName { get; set; }

        [StringLength(32)]
       [ProtoMember(15)]
        [RegularExpression(@"^\+?[\d]{7,}$", ResourceName = "InvalidPhone")]
        public string Phone { get; set; }

        [Required]
        [Email(ResourceName = "InvalidEmail")]
     
        [Remote("IsEmailAvailable", "CheckEmailAddress", "user")]
        [ProtoMember(16)]
        public string EmailAddress { get; set; }

        [Required]
        [StringLength(32)]
       [ProtoMember(17)]
        [RegularExpression(@"^[^-\s][أ-يa-zA-Z0-9_\s-]+$", ResourceName = "InvalidName")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(32)]
       [ProtoMember(18)]
        [RegularExpression(@"^[^-\s][أ-يa-zA-Z0-9_\s-]+$", ResourceName = "InvalidName")]
        public string LastName { get; set; }

        [StringLength(250)]
       [ProtoMember(19)]
        public string Company { get; set; }

       [ProtoMember(20)]
        [StringLength(250)]
        public string Address1 { get; set; }

        [StringLength(250)]
       [ProtoMember(21)]
        public string Address2 { get; set; }

        [StringLength(50)]
       [ProtoMember(22)]
        public string City { get; set; }

        [StringLength(50)]
       [ProtoMember(23)]
        public string State { get; set; }

        [StringLength(16)]
       [ProtoMember(24)]
        public string Postal { get; set; }

        [Required]
       [ProtoMember(25)]
        public bool IsAllowNotifications { get; set; }

        [RegularExpression(@"((?=.*\d)(?=.*\W+))(?![.\n])(?=.*[A-Z])(?=.*[a-z]).*$", ResourceName = "ComplexPass")]
        [StringLength(16, 6, ResourceName = "InvalidPassword")]
        [Required]
       [ProtoMember(26)]
        public string Password { get; set; }

        //[RegularExpression(@"((?=.*\d)|(?=.*\W+))(?![.\n])(?=.*[A-Z])(?=.*[a-z]).*$", ResourceName = "ComplexPass")]
        //[StringLength(16, 6, ResourceName = "InvalidPassword")]
        [CompareAttribute("Password", ResourceName = "PasswordAndConfirmPasswordMatch")]
        [Required]
        public string ConfirmPassword { get; set; }

        [Required]
       [ProtoMember(27)]
        public int Country { get; set; }

        [Required]
       [ProtoMember(28)]
        public int Language { get; set; }

       [ProtoMember(29)]
        public string ActivationCode { get; set; }

       [ProtoMember(30)]
        public string UserAgreementVersion { get; set; }


       [ProtoMember(31)]
        public string Invitationcode { get; set; }
       [ProtoMember(32)]
        public string requestCode { get; set; }
       [ProtoMember(33)]
        
        public bool AllowAPIAccess { get; set; }
       [ProtoMember(34)]
        public bool IsPrimaryUser { get; set; }

       [ProtoMember(35)]
        public int ImageId { get; set; }

       [ProtoMember(36)]
        public bool IsAccountDSP { get; set; }

       [ProtoMember(37)]
        public string IPAddress { get; set; }
       [ProtoMember(38)]
        public bool TaggingAllowed { get; set; }
       [ProtoMember(39)]
        public bool DisallowGeofenceLessThanRadius { get; set; }
       [ProtoMember(40)]
        public  bool FingerPrintAllowed { get; set; }
       [ProtoMember(41)]
        public  bool AllowExchangeCreativeFormat { get; set; }

       [ProtoMember(42)]
        public string Name { get { return this.ToString(); } set { } }
        public override string ToString()
        {
            return string.Format("{0} {1}", FirstName, LastName);
        }


       [ProtoMember(43)]
        public bool MyUsers { get; set; }
    }
}
