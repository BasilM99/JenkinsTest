using System;
using System.ComponentModel.DataAnnotations;
using ProtoBuf;

namespace ArabyAds.AdFalcon.Services.Interfaces.DTOs.Core
{
    [ProtoContract]
    public class DiscountDto
    {
       [ProtoMember(1)]
        [Framework.DataAnnotations.RegularExpression("^\\$?\\d+(\\.(\\d{1,5}))?$", ResourceName = "CurrencyMsg")]
        //[Framework.DataAnnotations.Required(ResourceName = "RequiredMessage")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:#####}")]
        public virtual decimal? Value { get; set; }
       [ProtoMember(2)]
        [Framework.DataAnnotations.Required(ResourceName = "RequiredMessage")]
        public virtual int TypeId { get; set; }
       [ProtoMember(3)]
        //[Framework.DataAnnotations.Required(ResourceName = "RequiredMessage")]
        public virtual DateTime? FromDate { get; set; }
       [ProtoMember(4)]
        public virtual DateTime? ToDate { get; set; }
    }
}
