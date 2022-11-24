using Noqoush.AdFalcon.Domain.Common.Model.Campaign;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Noqoush.AdFalcon.Domain.Common.Model.Core
{
    public enum CostModelWrapperEnum
    {
        CPC = 1, // Cost Per Click
        CPM = 2, // Cost Per Mile
        CPPV = 3, // Cost Per Page View
        CPV = 4,// Cost Per View
        CPI = 5, // Cost Per Install,
               CPA = 6// Cost Per Action
    }
    [DataContract(Name = "AppScope")]
    public enum AppScope
    {
        [EnumMember]
        Both = 0,
        [EnumMember]
        Network = 1,
        [EnumMember]
        DSP  = 2, 
      
    }
    

    //public class CostModelWrapper : ManagedLookupBase
    //{
    //    public virtual CostModel CostModel { get; set; }
    //    public virtual int DefaultBidValue { get; set; }

    //    public virtual int DefaultDSPBidValue { get; set; }
    //    public virtual int Factor { get; set; }

     
    //    public virtual TrackingEvent Event { get; set; }
    //}
}
