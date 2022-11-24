
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Noqoush.Framework.DataAnnotations;
using Noqoush.AdFalcon.Domain.Common.Model.Core;

using Noqoush.AdFalcon.Domain.Common.Model.Account;
using Noqoush.AdFalcon.Common;

namespace Noqoush.AdFalcon.Services.Interfaces.DTOs.Account
{
    [DataContract]
    public class AccountDSPRequestDto
    {
        [DataMember]
        public string CountryNameValue { get; set; }
        [DataMember]
        public string CompanyTypeNameValue { get; set; }

        [DataMember]
        public bool IsApproved { get; set; }
        [DataMember]
        public int Id { get; set; }
        [StringLength(1000)]
        [DataMember]
        public string Note { get; set; }

        [StringLength(1000)]
        [DataMember]
        public string ActionNote { get; set; }

        
        [DataMember]
        public int AccountId { get; set; }
        [DataMember]
        public string StatusName { get {

                return this.Status.ToText();


            } set {



            } }
        [DataMember]
        public string AccountName { get; set; }

        [StringLength(32)]
        [DataMember]
        [RegularExpression(@"^\+?[\d]{7,}$", ResourceName = "InvalidPhone")]
        public string Phone { get; set; }

        [Required]
        [Email(ResourceName = "InvalidEmail")]
        [DataMember]
        [Noqoush.Framework.DataAnnotations.Remote("InvalidEmailDSP", "CheckAccountDSPEmailAddress", "user")]
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
        [Required]
        [StringLength(250)]
        [DataMember]
        public string Company { get; set; }

        [DataMember]
        [StringLength(250)]
        public string Address1 { get; set; }

        [StringLength(250)]
        [DataMember]
        public string Address2 { get; set; }

        [Required]
        [DataMember]
        public int Country { get; set; }
        [DataMember]
        [Required]
        public int CompanyType { get; set; }
        [DataMember]
        public DateTime ActionDate { get; set; }
        [DataMember]
        public DateTime RequestDate { get; set; }
        [DataMember]
        public AccountDSPRequestStatus Status { get; set; }

        [DataMember]
        public bool IsAllowNotifications { get; set; }

        [DataMember]
        public bool Result { get; set; }

        [DataMember]
        public int ImageId { get; set; }


        public dynamic CompanyTypes { get; set; }

        public dynamic AccountDSPStatus { get; set; }
        public override string ToString()
        {
            return string.Format("{0} {1}", FirstName, LastName);
        }
    }
}
