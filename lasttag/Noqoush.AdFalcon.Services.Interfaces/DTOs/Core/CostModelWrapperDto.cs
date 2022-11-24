using Noqoush.AdFalcon.Domain.Common.Model.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Noqoush.AdFalcon.Services.Interfaces.DTOs.Core
{
    [DataContract]
    public class CostModelWrapperDto : LookupDto
    {
        [DataMember]
        public int CostModel { get; set; }

        [DataMember]
        public int Factor { get; set; }
        

        [DataMember]
        public AppScope Scope { get; set; }
    }
}
