using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using Noqoush.AdFalcon.Domain.Common.Model.Core.CostElement;
using Noqoush.Framework.DataAnnotations;


namespace Noqoush.AdFalcon.Services.Interfaces.DTOs.Core
{
    /*public enum CostElementTypeDto
    {
        Percentage = 1,
        Fixed = 2,
    }*/
    [DataContract]
    public class FeeDto : LookupDto
    {
        [DataMember]
        public virtual CostItemType CostItemType { get; set; }
        [DataMember]
        public virtual bool IsBillable { get; set; }

        [DataMember]
        public virtual bool IsAutoAdded { get; set; }

        [DataMember]
        public virtual bool IsSystemCalcu { get; set; }
        [DataMember]
        public virtual FeeCalculatedFrom FeeCalculatedFrom { get; set; }
        [DataMember]
        public virtual long CalculatedFromFeeCategory { get; set; }
        [DataMember]
        public virtual CostElementCalculatedFrom CostElementCalculatedFrom { get; set; }
        [DataMember]
        public virtual long Category { get; set; }

        [DataMember]
        public virtual int TypeId { get; set; }
        [DataMember]
        public virtual bool IsOneTime { get; set; }
        [DataMember]
        public virtual IEnumerable<CostElementValueDto> Values { get; set; }
        [DataMember]
        public virtual bool IsData { get; set; }
        [DataMember]
        public virtual bool IsThirdParty { get; set; }

        [DataMember]
        public virtual bool IsPlatform { get; set; }
        [DataMember]
        public virtual bool IsAVR { get; set; }

        [DataMember]
        public virtual bool IsDataFee { get; set; }
        [DataMember]
        public virtual bool IsThirdPartyFee { get; set; }

        [DataMember]
        public virtual bool IsPlatformFee { get; set; }
        [DataMember]
        public virtual bool IsAVRFee { get; set; }
        [DataMember]
        public virtual bool IsExchangeDiscrepancy { get; set; }

        [DataMember]
        public virtual bool IsExchangeDiscrepancyFee { get; set; }
        [DataMember]
        public virtual int Scope { get; set; }
    }
    [DataContract]

    public class CostElementResultDto
    {
        [DataMember]
        public IEnumerable<CostElementDto> Items { get; set; }
        [DataMember]
        public long TotalCount { get; set; }
    }
    public class CostElementDto : LookupDto
    {
        [DataMember]
        public virtual CostItemType CostItemType { get; set; }
        [DataMember]
        public virtual bool IsBillable { get; set; }
        [DataMember]
        public virtual FeeCalculatedFrom FeeCalculatedFrom { get; set; }
        [DataMember]
        public virtual long CalculatedFromFeeCategory { get; set; }
        [DataMember]
        public virtual CostElementCalculatedFrom CostElementCalculatedFrom { get; set; }
        [DataMember]
        public virtual long Category { get; set; }

        [DataMember]
        public virtual int TypeId { get; set; }

        [DataMember]
        public virtual int Scope { get; set; }
        [DataMember]
        public virtual bool IsOneTime { get; set; }
        [DataMember]
        public virtual IEnumerable<CostElementValueDto> Values { get; set; }
        [DataMember]
        public virtual bool IsData { get; set; }
        [DataMember]
        public virtual bool IsThirdParty { get; set; }

        [DataMember]
        public virtual bool IsPlatform { get; set; }
        [DataMember]
        public virtual bool IsAVR { get; set; }

        [DataMember]
        public virtual bool IsDataFee { get; set; }
        [DataMember]
        public virtual bool IsThirdPartyFee { get; set; }
        [DataMember]
        public virtual bool IsAutoAdded { get; set; }
        [DataMember]
        public virtual bool IsPlatformFee { get; set; }
        [DataMember]
        public virtual bool IsAVRFee { get; set; }
        [DataMember]
        public virtual bool IsExchangeDiscrepancy { get; set; }

        [DataMember]
        public virtual bool IsExchangeDiscrepancyFee { get; set; }

   
    }

    [DataContract]
    public class CostElementValueDto
    {
        [DataMember]
        public virtual decimal Value { get; set; }
        [DataMember]
        public virtual CostModelWrapperDto CostModelWrapper { get; set; }
    }
}
