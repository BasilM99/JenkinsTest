using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Noqoush.AdFalcon.Services.Interfaces.DTOs.Core
{
    [DataContract]
    public class CountryDto : LookupDto
    {
        [DataMember]
        public virtual string Code { get; set; }
    }
}
