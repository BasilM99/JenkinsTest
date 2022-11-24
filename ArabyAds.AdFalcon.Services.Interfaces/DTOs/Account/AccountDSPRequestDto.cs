
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProtoBuf;
using ArabyAds.Framework.DataAnnotations;
using ArabyAds.AdFalcon.Domain.Common.Model.Core;

using ArabyAds.AdFalcon.Domain.Common.Model.Account;
using ArabyAds.AdFalcon.Common;

namespace ArabyAds.AdFalcon.Services.Interfaces.DTOs.Account
{
    [ProtoContract]
    public class AccountDSPRequestDto
    {
       [ProtoMember(1)]
        public string CountryNameValue { get; set; }
       [ProtoMember(2)]
        public string CompanyTypeNameValue { get; set; }

       [ProtoMember(3)]
        public bool IsApproved { get; set; }
       [ProtoMember(4)]
        public int Id { get; set; }
        [StringLength(1000)]
       [ProtoMember(5)]
        public string Note { get; set; }

        [StringLength(1000)]
       [ProtoMember(6)]
        public string ActionNote { get; set; }

        
       [ProtoMember(7)]
        public int AccountId { get; set; }
       [ProtoMember(8)]
        public string StatusName { get {

                return this.Status.ToText();


            } set {



            } }
       [ProtoMember(9)]
        public string AccountName { get; set; }

        [StringLength(32)]
       [ProtoMember(10)]
        [RegularExpression(@"^\+?[\d]{7,}$", ResourceName = "InvalidPhone")]
        public string Phone { get; set; }

        [Required]
        [Email(ResourceName = "InvalidEmail")]
       [ProtoMember(11)]
        [ArabyAds.Framework.DataAnnotations.Remote("InvalidEmailDSP", "CheckAccountDSPEmailAddress", "user")]
        public string EmailAddress { get; set; }

        [Required]
        [StringLength(32)]
       [ProtoMember(12)]
        [RegularExpression(@"^[^-\s][أ-يa-zA-Z0-9_\s-]+$", ResourceName = "InvalidName")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(32)]
       [ProtoMember(13)]
        [RegularExpression(@"^[^-\s][أ-يa-zA-Z0-9_\s-]+$", ResourceName = "InvalidName")]
        public string LastName { get; set; }
        [Required]
        [StringLength(250)]
       [ProtoMember(14)]
        public string Company { get; set; }

       [ProtoMember(15)]
        [StringLength(250)]
        public string Address1 { get; set; }

        [StringLength(250)]
       [ProtoMember(16)]
        public string Address2 { get; set; }

        [Required]
       [ProtoMember(17)]
        public int Country { get; set; }
       [ProtoMember(18)]
        [Required]
        public int CompanyType { get; set; }
       [ProtoMember(19)]
        public DateTime ActionDate { get; set; }
       [ProtoMember(20)]
        public DateTime RequestDate { get; set; }
       [ProtoMember(21)]
        public AccountDSPRequestStatus Status { get; set; }

       [ProtoMember(22)]
        public bool IsAllowNotifications { get; set; }

       [ProtoMember(23)]
        public bool Result { get; set; }

       [ProtoMember(24)]
        public int ImageId { get; set; }


        public dynamic CompanyTypes { get; set; }

        public dynamic AccountDSPStatus { get; set; }
        public override string ToString()
        {
            return string.Format("{0} {1}", FirstName, LastName);
        }
    }
}
