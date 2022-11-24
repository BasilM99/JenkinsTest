
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;


namespace Noqoush.AdFalcon.Domain.Common.Model.Core.CostElement
{
    [Serializable]
    [DataContract()]
    public enum CalculationType
    {
        [EnumMember]
        Undefined = 0,
        [EnumMember]
        [EnumText("PercentageType", "Lookup")]
        Percentage = 1,
        [EnumMember]
        [EnumText("FixedType", "Lookup")]
        Fixed = 2,
    }
    [Serializable]
    [DataContract()]
    public enum CostItemType
    {
        [EnumMember]
        Undefined = 0,
        [EnumMember]
        [EnumText("CostElement", "CostItemType")]
        // [EnumText("PercentageType", "Lookup")]
        CostElement = 1,
        [EnumMember]
        [EnumText("Fee", "CostItemType")]
        //[EnumText("FixedType", "Lookup")]
        Fee = 2,
    }

    [Serializable]
    [DataContract()]
    public enum CostElementCalculatedFrom
    {
        [EnumMember]
        Undefined =0,
        [EnumMember]
        [EnumText("Fee", "CostElementCalculatedFrom")]
        // [EnumText("PercentageType", "Lookup")]
        Fee = 1,
    
        [EnumMember]
        [EnumText("NetCost", "CostElementCalculatedFrom")]
        //[EnumText("FixedType", "Lookup")]
        NetCost = 2,

        [EnumMember]
        [EnumText("BillableCost", "CostElementCalculatedFrom")]
        BillableCost = 3,
    }

    [Serializable]
    [DataContract()]
    public enum FeeCalculatedFrom
    {
        [EnumMember]
        Undefined = 0,
        [EnumMember]
        [EnumText("ANC", "FeeCalculatedFrom")]
        // [EnumText("PercentageType", "Lookup")]
        ANC = 1,
        [EnumMember]
        [EnumText("System", "FeeCalculatedFrom")]
        //[EnumText("FixedType", "Lookup")]
        System = 2,
  
    }
    [Serializable]
    [DataContract()]
    [Flags]
    public enum CostItemCategroyFlags

    {
        [EnumMember]
        Undefined =0,

        [EnumMember]
        [EnumText("Data", "CostItemCategroyFlags")]
        Data = 1,
        [EnumMember]
        [EnumText("ThirdParty", "CostItemCategroyFlags")]
        ThirdParty = 2,
        [EnumMember]
        [EnumText("Platform", "CostItemCategroyFlags")]
        Platform = 4,
        [EnumMember]
        [EnumText("AVR", "CostItemCategroyFlags")]
        AVR = 8,
        [EnumMember]
        [EnumText("ExchangeDiscrepancy", "CostItemCategroyFlags")]
        ExchangeDiscrepancy =16

    }
    //public class CostItem :  ManagedLookupBase
    //{
    //    public virtual long Category { get; set; }
    //    public virtual CalculationType Type { get; set; }

    //    public virtual CostItemType CostItemType { get; set; }
        
    //    public virtual IEnumerable<CostItemValue> Values { get; set; }
    //    //public virtual string GetDescription()
    //    //{
    //    //    return this.GetDescription();
    //    //}

    //}
}
