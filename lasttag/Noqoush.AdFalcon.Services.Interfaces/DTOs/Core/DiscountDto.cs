using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Noqoush.AdFalcon.Services.Interfaces.DTOs.Core
{
    [DataContract]
    public class DiscountDto
    {
        [DataMember]
        [Framework.DataAnnotations.RegularExpression("^\\$?\\d+(\\.(\\d{1,5}))?$", ResourceName = "CurrencyMsg")]
        //[Framework.DataAnnotations.Required(ResourceName = "RequiredMessage")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:#####}")]
        public virtual decimal? Value { get; set; }
        [DataMember]
        [Framework.DataAnnotations.Required(ResourceName = "RequiredMessage")]
        public virtual int TypeId { get; set; }
        [DataMember]
        //[Framework.DataAnnotations.Required(ResourceName = "RequiredMessage")]
        public virtual DateTime? FromDate { get; set; }
        [DataMember]
        public virtual DateTime? ToDate { get; set; }
    }
}
