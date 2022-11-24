using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Noqoush.AdFalcon.Domain.Common.Model.Campaign;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Core;

namespace Noqoush.AdFalcon.Services.Interfaces.DTOs.Campaign.Creative
{
   
        [DataContract]
        public class CreativeUnitGroupDto : LookupDto
        {
        [DataMember]
        public string Code { get; set; }

        [DataMember]
        public virtual int MaxSize { get; set; }


    }
}
