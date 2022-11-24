using Noqoush.AdFalcon.Domain.Common.Model.Account;
using Noqoush.AdFalcon.Domain.Common.Model.Account;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Reports;
using Noqoush.Framework.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Noqoush.AdFalcon.Services.Interfaces.DTOs.Account
{
    [DataContract]
   public class AccountDSPsettingDTO
    {
      
        [DataMember]
        public bool IsDeleted { get; set; }
        [DataMember]
        public int StateId { get; set; }
        [DataMember]
        [Required]
        public int CountryId { get; set; }

        [DataMember]
        public int ID { get; set; }
        [DataMember]
        public int AccountId { get; set; }
        [DataMember]
        [Required]
        public  string BillToAddressPersonName { get; set; }
        [DataMember]
        [Required]
        public  string BillToAddress1 { get; set; }
        [DataMember]
        [Required]
        public  string BillToAddress2 { get; set; }
        [DataMember]
        [Required]
        public string BillingContactName { get; set; }

        [DataMember]
        [Required]
        public string BusinessName { get; set; }

        [DataMember]
   
        public AgencyCommission AgencyCommission { get; set; }

        [DataMember]
        public IList<AccountDSPsettingContactDTO> AllContacts { get; set; }
    }


    [DataContract]

    public class AccountDSPsettingContactDTO : RecipientEmailDTO
    {

        [DataMember]
        public int AccountSettingId { get; set; }
    }
}
