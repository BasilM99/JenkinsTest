using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Noqoush.AdFalcon.Services.Interfaces.DTOs.Core
{
    [DataContract]
    public class OperatorDto
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public LocalizedStringDto Name { get; set; }
    }
}
