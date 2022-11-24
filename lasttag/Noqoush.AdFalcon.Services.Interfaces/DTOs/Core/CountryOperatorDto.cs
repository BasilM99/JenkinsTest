using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Noqoush.AdFalcon.Services.Interfaces.DTOs.Core
{
    [DataContract]
    public class CountryOperatorDto
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public LocalizedStringDto Name { get; set; }
        [DataMember]
        public List<OperatorDto> Operators { get; set; }
    }
}
