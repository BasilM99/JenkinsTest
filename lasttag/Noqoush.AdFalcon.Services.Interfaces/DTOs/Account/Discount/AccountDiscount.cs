using System.Runtime.Serialization;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Core;

namespace Noqoush.AdFalcon.Services.Interfaces.DTOs.Account.Discount
{
    [DataContract]
    public class AccountDiscountDto 
    {
        [DataMember]
        public virtual int AccountId { get; set; }
        [DataMember]
        public virtual DiscountDto Discount { get; set; }
        [DataMember]
        public virtual int ID { get; set; }
    }
}
