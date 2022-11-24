using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Noqoush.Framework.DataAnnotations;
using Noqoush.AdFalcon.Domain.Common.Model.Core;
using Noqoush.AdFalcon.Domain.Common.Model.Account;

namespace Noqoush.AdFalcon.Services.Interfaces.DTOs.Account
{
    [DataContract]
    public class UserDto
    {
        [DataMember]
        public UserType UserType { get; set; }
        [DataMember]
        public bool IsSecondPrimaryUser { get; set; }
        [DataMember]
        public bool AlreadyReg { get; set; }
        [DataMember]
        public decimal VATValue { get; set; }
        [DataMember]
        public int AccountRole { get; set; }
        [DataMember]
        public bool ExchangeChecked { get; set; }
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public int AppSiteId { get; set; }
        [DataMember]
        public string buyerCode { get; set; }
        [DataMember]
        public int? buyerId { get; set; }

        [DataMember]
        public bool Block { get; set; }
        [DataMember]
        public int AccountId { get; set; }
        [DataMember]
        public bool  Checked { get; set; }
        [DataMember]
        public string AccountName { get; set; }

        [StringLength(32)]
        [DataMember]
        [RegularExpression(@"^\+?[\d]{7,}$", ResourceName = "InvalidPhone")]
        public string Phone { get; set; }

        [Required]
        [Email(ResourceName = "InvalidEmail")]
        [DataMember]
        [RemoteAttribute("IsEmailAvailable", "CheckEmailAddress", "user")]
        public string EmailAddress { get; set; }

        [Required]
        [StringLength(32)]
        [DataMember]
        [RegularExpression(@"^[^-\s][أ-يa-zA-Z0-9_\s-]+$", ResourceName = "InvalidName")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(32)]
        [DataMember]
        [RegularExpression(@"^[^-\s][أ-يa-zA-Z0-9_\s-]+$", ResourceName = "InvalidName")]
        public string LastName { get; set; }

        [StringLength(250)]
        [DataMember]
        public string Company { get; set; }

        [DataMember]
        [StringLength(250)]
        public string Address1 { get; set; }

        [StringLength(250)]
        [DataMember]
        public string Address2 { get; set; }

        [StringLength(50)]
        [DataMember]
        public string City { get; set; }

        [StringLength(50)]
        [DataMember]
        public string State { get; set; }

        [StringLength(16)]
        [DataMember]
        public string Postal { get; set; }

        [Required]
        [DataMember]
        public bool IsAllowNotifications { get; set; }

        [RegularExpression(@"((?=.*\d)|(?=.*\W+))(?![.\n])(?=.*[A-Z])(?=.*[a-z]).*$", ResourceName = "ComplexPass")]
        [StringLength(16, 6, ResourceName = "InvalidPassword")]
        [Required]
        [DataMember]
        public string Password { get; set; }

        //[RegularExpression(@"((?=.*\d)|(?=.*\W+))(?![.\n])(?=.*[A-Z])(?=.*[a-z]).*$", ResourceName = "ComplexPass")]
        //[StringLength(16, 6, ResourceName = "InvalidPassword")]
        [CompareAttribute("Password", ResourceName = "PasswordAndConfirmPasswordMatch")]
        [Required]
        public string ConfirmPassword { get; set; }

        [Required]
        [DataMember]
        public int Country { get; set; }

        [Required]
        [DataMember]
        public int Language { get; set; }

        [DataMember]
        public string ActivationCode { get; set; }

        [DataMember]
        public string UserAgreementVersion { get; set; }


        [DataMember]
        public string Invitationcode { get; set; }
        [DataMember]
        public string requestCode { get; set; }
        [DataMember]
        
        public bool AllowAPIAccess { get; set; }
        [DataMember]
        public bool IsPrimaryUser { get; set; }

        [DataMember]
        public int ImageId { get; set; }

        [DataMember]
        public bool IsAccountDSP { get; set; }

        [DataMember]
        public string IPAddress { get; set; }
        [DataMember]
        public bool TaggingAllowed { get; set; }
        [DataMember]
        public bool DisallowGeofenceLessThanRadius { get; set; }
        [DataMember]
        public  bool FingerPrintAllowed { get; set; }
        [DataMember]
        public  bool AllowExchangeCreativeFormat { get; set; }

        [DataMember]
        public string Name { get { return this.ToString(); } set { } }
        public override string ToString()
        {
            return string.Format("{0} {1}", FirstName, LastName);
        }


        [DataMember]
        public bool MyUsers { get; set; }
    }
}
