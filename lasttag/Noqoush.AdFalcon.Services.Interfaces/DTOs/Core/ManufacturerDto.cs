using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Noqoush.AdFalcon.Services.Interfaces.DTOs.Core
{
    [DataContract]
    public class ManufacturerDto : LookupDto
    {
        [DataMember]
        [Noqoush.Framework.DataAnnotations.Required()]
        public int Order { get; set; }
    }
}
