using System;
using System.Collections.Generic;
using System.Linq;
using ProtoBuf;
using System.Text;
using ArabyAds.AdFalcon.Domain.Common.Model.Core.CostElement;
using ArabyAds.Framework.DataAnnotations;


namespace ArabyAds.AdFalcon.Services.Interfaces.DTOs.Core
{
    /*public enum CostElementTypeDto
    {
        Percentage = 1,
        Fixed = 2,
    }*/
    [ProtoContract]
    public class FeeDto : LookupDto
    {
        [ProtoMember(1)]
        public virtual CostItemType CostItemType { get; set; }
        [ProtoMember(2)]
        public virtual bool IsBillable { get; set; }

        [ProtoMember(3)]
        public virtual bool IsAutoAdded { get; set; }

        [ProtoMember(4)]
        public virtual bool IsSystemCalcu { get; set; }
        [ProtoMember(5)]
        public virtual FeeCalculatedFrom FeeCalculatedFrom { get; set; }
        [ProtoMember(6)]
        public virtual long CalculatedFromFeeCategory { get; set; }
        [ProtoMember(7)]
        public virtual CostElementCalculatedFrom CostElementCalculatedFrom { get; set; }
        [ProtoMember(8)]
        public virtual long Category { get; set; }

        [ProtoMember(9)]
        public virtual int TypeId { get; set; }
        [ProtoMember(10)]
        public virtual bool IsOneTime { get; set; }
        [ProtoMember(11)]
        public virtual IEnumerable<CostElementValueDto> Values { get; set; } = new List<CostElementValueDto>();
        [ProtoMember(12)]
        public virtual bool IsData { get; set; }
        [ProtoMember(13)]
        public virtual bool IsThirdParty { get; set; }

        [ProtoMember(14)]
        public virtual bool IsPlatform { get; set; }
        [ProtoMember(15)]
        public virtual bool IsAVR { get; set; }

        [ProtoMember(16)]
        public virtual bool IsDataFee { get; set; }
        [ProtoMember(17)]
        public virtual bool IsThirdPartyFee { get; set; }

        [ProtoMember(18)]
        public virtual bool IsPlatformFee { get; set; }
        [ProtoMember(19)]
        public virtual bool IsAVRFee { get; set; }
        [ProtoMember(20)]
        public virtual bool IsExchangeDiscrepancy { get; set; }

        [ProtoMember(21)]
        public virtual bool IsExchangeDiscrepancyFee { get; set; }
        [ProtoMember(22)]
        public virtual int Scope { get; set; }
        [ProtoMember(23)]
        public virtual bool IsAdfalconRevenue { get; set; }

        [ProtoMember(24)]
        public virtual bool IsAdfalconRevenueFee { get; set; }

    }
    [ProtoContract]

    public class CostElementResultDto
    {
        [ProtoMember(1)]
        public IEnumerable<CostElementDto> Items { get; set; } = new List<CostElementDto>();
        [ProtoMember(2)]
        public long TotalCount { get; set; }
    }

    [ProtoContract]

    public class CostElementDto : LookupDto
    {
        [ProtoMember(1)]
        public virtual CostItemType CostItemType { get; set; }
        [ProtoMember(2)]
        public virtual bool IsBillable { get; set; }
        [ProtoMember(3)]
        public virtual FeeCalculatedFrom FeeCalculatedFrom { get; set; }
        [ProtoMember(4)]
        public virtual long CalculatedFromFeeCategory { get; set; }
        [ProtoMember(5)]
        public virtual CostElementCalculatedFrom CostElementCalculatedFrom { get; set; }
        [ProtoMember(6)]
        public virtual long Category { get; set; }

        [ProtoMember(7)]
        public virtual int TypeId { get; set; }

        [ProtoMember(8)]
        public virtual int Scope { get; set; }
        [ProtoMember(9)]
        public virtual bool IsOneTime { get; set; }
        [ProtoMember(10)]
        public virtual IEnumerable<CostElementValueDto> Values { get; set; } = new List<CostElementValueDto>();
        [ProtoMember(11)]
        public virtual bool IsData { get; set; }
        [ProtoMember(12)]
        public virtual bool IsThirdParty { get; set; }

        [ProtoMember(13)]
        public virtual bool IsPlatform { get; set; }
        [ProtoMember(14)]
        public virtual bool IsAVR { get; set; }

        [ProtoMember(15)]
        public virtual bool IsDataFee { get; set; }
        [ProtoMember(16)]
        public virtual bool IsThirdPartyFee { get; set; }
        [ProtoMember(17)]
        public virtual bool IsAutoAdded { get; set; }
        [ProtoMember(18)]
        public virtual bool IsPlatformFee { get; set; }
        [ProtoMember(19)]
        public virtual bool IsAVRFee { get; set; }
        [ProtoMember(20)]
        public virtual bool IsExchangeDiscrepancy { get; set; }

        [ProtoMember(21)]
        public virtual bool IsExchangeDiscrepancyFee { get; set; }


        [ProtoMember(22)]
        public virtual bool IsAdfalconRevenue { get; set; }

        [ProtoMember(23)]
        public virtual bool IsAdfalconRevenueFee { get; set; }


    }

    [ProtoContract]
    public class CostElementValueDto
    {
        [ProtoMember(1)]
        public virtual decimal Value { get; set; }
        [ProtoMember(2)]
        public virtual CostModelWrapperDto CostModelWrapper { get; set; }
    }
}
