using System;
using System.Collections.Generic;
using System.Linq;
using ProtoBuf;
using System.Text;
using System.Threading.Tasks;

namespace ArabyAds.AdFalcon.Services.Interfaces.DTOs.Account.SSP
{

    [ProtoContract]
    public class PartnerDto
    {
       [ProtoMember(1)]
        public int ID { get; set; }

       [ProtoMember(2)]
        public string Name { get; set; }

       [ProtoMember(3)]
        public string Email { get; set; }
       [ProtoMember(4)]
        public string ContactPerson { get; set; }
       [ProtoMember(5)]
        public string AccountName { get; set; }

    }


    [ProtoContract]
    public class ResultPartnerDto
    {
        [ProtoMember(1)]
        public List<PartnerDto> Items { get; set; } = new List<PartnerDto>();
       [ProtoMember(2)]
        public long TotalCount { get; set; }

    }

}
