using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Noqoush.AdFalcon.Services.Interfaces.DTOs.Account.SSP
{

    [DataContract]
    public class PartnerDto
    {
        [DataMember]
        public int ID { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Email { get; set; }
        [DataMember]
        public string ContactPerson { get; set; }
        [DataMember]
        public string AccountName { get; set; }

    }


    [DataContract]
    public class ResultPartnerDto
    {
        [DataMember]
        public List<PartnerDto> Items { get; set; }
        [DataMember]
        public long TotalCount { get; set; }

    }

}
