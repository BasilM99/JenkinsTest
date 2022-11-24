using ArabyAds.AdFalcon.Domain.Common.Model.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using ProtoBuf;
using System.Text;
using System.Threading.Tasks;

namespace ArabyAds.AdFalcon.Services.Interfaces.DTOs.Campaign.Objective
{
    
   

  
    [ProtoContract]
    public class      AdActionCostModelWrapperDto
 
    {
        
       [ProtoMember(1)]
        public int CostModelWrapperId { get; set; }

       [ProtoMember(2)]
        public AppScope Scope { get; set; }
    }
}
