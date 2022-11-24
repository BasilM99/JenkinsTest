using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Noqoush.Framework.DataAnnotations;
using System.Runtime.Serialization;

namespace Noqoush.AdFalcon.Web.Controllers.Model
{
    [DataContract]

    public class AdFundDto
    {
        [Required]
        [StringLength(25)]
        public string Name { get; set; }

        [Required]
        [Email(ResourceName = "InvalidEmail")]
        public string Email { get; set; }

        [Required]
        [StringLength(250)]
        public string Comment { get; set; }

        [Required]
        [RegularExpression(@"^[\d\-\s]{0,}$", ResourceName = "InvalidPhoneAdFund")]
        public string PhoneNumber { get; set; }
    }
    [DataContract]

    public class AdFundDtoPGW
    {
        [DataMember]
        [Required(ResourceName = "RequiredFund")]
        [Range(500, 99999999, ResourceName = "MaxFund")]
        [RegularExpression(@"^\$?\d+?$", ResourceName = "FundCurrencyMsg")]
        public int Amount { get; set; }
   
        public string PaymentType { get; set; }
        [DataMember]

        public decimal VatAmount
        {
            get; set;
        }
        [DataMember]
        public double NetAmount
        {
            get; set;
        }
    }
}
