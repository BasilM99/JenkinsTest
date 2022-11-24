using System;
using System.Runtime.Serialization;
using Noqoush.AdFalcon.Domain.Common.Model.AppSite;
using Noqoush.Framework.DataAnnotations;

namespace Noqoush.AdFalcon.Services.Interfaces.DTOs.AppSite
{
    [DataContract]
    public class AppSiteRevenueCalculationSettingDto
    {
        [DataMember]
        public virtual int ID { get; set; }
        [DataMember]
        public virtual CalculationMode CalculationMode { get; set; }
        [DataMember]
        [Range(0, 100, ResourceName = "RangeMessage0100")]
        [System.ComponentModel.DataAnnotations.DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0}")]
        public virtual decimal? Value { get; set; }
        [DataMember]
        public virtual DateTime FromDate { get; set; }
        [DataMember]
        public virtual DateTime? ToDate { get; set; }

    }
}
