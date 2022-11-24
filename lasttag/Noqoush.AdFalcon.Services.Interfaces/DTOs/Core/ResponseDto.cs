using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Noqoush.AdFalcon.Services.Interfaces.DTOs.Core
{
    [DataContract]

    public class ResponseDto
    {
        [DataMember]
        public string Massage { get; set; }
        [DataMember]
        public bool success { get; set; }

    }
}
