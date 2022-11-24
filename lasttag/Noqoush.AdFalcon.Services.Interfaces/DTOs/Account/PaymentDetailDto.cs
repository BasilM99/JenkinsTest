using System.Runtime.Serialization;

namespace Noqoush.AdFalcon.Services.Interfaces.DTOs.Account
{
    [DataContract]
    public class PaymentDetailDto
    {
        [DataMember]
        public int ID { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string IsDefault { get; set; }
    }

    [DataContract]
    public class PaymentFullDetailDto : PaymentDetailDto
    {
       
        [DataMember]
        public int TypeId { get; set; }

        [DataMember]
        public string BeneficiaryName{get;set;}

        [DataMember]
        public string BankName{get;set;}

        [DataMember]
        public string UserName{get;set;}

        [DataMember]
        public string BankAddress{get;set;}

        [DataMember]
        public string RecipientAccountNumber{get;set;}

        [DataMember]
        public string SWIFT{get;set;}
    }
}
