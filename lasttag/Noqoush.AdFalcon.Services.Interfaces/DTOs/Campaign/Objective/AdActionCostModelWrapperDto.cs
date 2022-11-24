using Noqoush.AdFalcon.Domain.Common.Model.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Noqoush.AdFalcon.Services.Interfaces.DTOs.Campaign.Objective
{
    
   

  
    [DataContract]
    public class      AdActionCostModelWrapperDto
 
    {
        
        [DataMember]
        public int CostModelWrapperId { get; set; }

        [DataMember]
        public AppScope Scope { get; set; }
    }
}
