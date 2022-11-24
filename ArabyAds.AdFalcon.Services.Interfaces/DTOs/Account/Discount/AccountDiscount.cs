using ProtoBuf;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Core;

namespace ArabyAds.AdFalcon.Services.Interfaces.DTOs.Account.Discount
{
    [ProtoContract]
    public class AccountDiscountDto 
    {
       [ProtoMember(1)]
        public virtual int AccountId { get; set; }
       [ProtoMember(2)]
        public virtual DiscountDto Discount { get; set; }
       [ProtoMember(3)]
        public virtual int ID { get; set; }
    }
}
