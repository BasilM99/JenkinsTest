using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Noqoush.AdFalcon.Services.Interfaces.DTOs.Reports.Performance
{
    [DataContract]
    public class AccountCriteria
    {
        [DataMember]
        public string Email { get; set; }

        [DataMember]
        public string CompanyName { get; set; }

        [DataMember]
        public string Name { get; set; }

    }
}
