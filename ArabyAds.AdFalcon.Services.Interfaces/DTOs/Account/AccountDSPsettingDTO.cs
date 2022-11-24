using ArabyAds.AdFalcon.Domain.Common.Model.Account;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Reports;
using ArabyAds.Framework.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using ProtoBuf;
using System.Text;
using System.Threading.Tasks;

namespace ArabyAds.AdFalcon.Services.Interfaces.DTOs.Account
{
    [ProtoContract]
   public class AccountDSPsettingDTO
    {
      
       [ProtoMember(1)]
        public bool IsDeleted { get; set; }
       [ProtoMember(2)]
        public int StateId { get; set; }
       [ProtoMember(3)]
        [Required]
        public int CountryId { get; set; }

       [ProtoMember(4)]
        public int ID { get; set; }
       [ProtoMember(5)]
        public int AccountId { get; set; }
       [ProtoMember(6)]
        [Required]
        public  string BillToAddressPersonName { get; set; }
       [ProtoMember(7)]
        [Required]
        public  string BillToAddress1 { get; set; }
       [ProtoMember(8)]
        [Required]
        public  string BillToAddress2 { get; set; }
       [ProtoMember(9)]
        [Required]
        public string BillingContactName { get; set; }

       [ProtoMember(10)]
        [Required]
        public string BusinessName { get; set; }

       [ProtoMember(11)]
   
        public AgencyCommission AgencyCommission { get; set; }

        [ProtoMember(12)]
        public IList<AccountDSPsettingContactDTO> AllContacts { get; set; } = new List<AccountDSPsettingContactDTO>();
    }


    [ProtoContract]

    public class AccountDSPsettingContactDTO : RecipientEmailDTO
    {

       [ProtoMember(1)]
        public int AccountSettingId { get; set; }
    }
}
