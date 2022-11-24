using ProtoBuf;

namespace ArabyAds.AdFalcon.Services.Interfaces.DTOs.Account
{
    [ProtoContract]
    [ProtoInclude(100,typeof(PaymentFullDetailDto))]
    public class PaymentDetailDto
    {
       [ProtoMember(1)]
        public int ID { get; set; }
       [ProtoMember(2)]
        public string Name { get; set; }
       [ProtoMember(3)]
        public string IsDefault { get; set; }
    }

    [ProtoContract]
    public class PaymentFullDetailDto : PaymentDetailDto
    {
       
       [ProtoMember(1)]
        public int TypeId { get; set; }

       [ProtoMember(2)]
        public string BeneficiaryName{get;set;}

       [ProtoMember(3)]
        public string BankName{get;set;}

       [ProtoMember(4)]
        public string UserName{get;set;}

       [ProtoMember(5)]
        public string BankAddress{get;set;}

       [ProtoMember(6)]
        public string RecipientAccountNumber{get;set;}

       [ProtoMember(7)]
        public string SWIFT{get;set;}
    }
}
