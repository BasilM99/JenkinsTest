using System;
using ProtoBuf;
using ArabyAds.AdFalcon.Domain.Common.Model.AppSite;
using ArabyAds.Framework.DataAnnotations;

namespace ArabyAds.AdFalcon.Services.Interfaces.DTOs.AppSite
{
    [ProtoContract]
    public class AppSiteRevenueCalculationSettingDto
    {
       [ProtoMember(1)]
        public virtual int ID { get; set; }
       [ProtoMember(2)]
        public virtual CalculationMode CalculationMode { get; set; }
       [ProtoMember(3)]
        [Range(0, 100, ResourceName = "RangeMessage0100")]
        [System.ComponentModel.DataAnnotations.DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0}")]
        public virtual decimal? Value { get; set; }
       [ProtoMember(4)]
        public virtual DateTime FromDate { get; set; }
       [ProtoMember(5)]
        public virtual DateTime? ToDate { get; set; }

    }
}
