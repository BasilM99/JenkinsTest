using System;
using System.Collections.Generic;
using System.Linq;
using ProtoBuf;
using System.Text;
using System.Threading.Tasks;

namespace ArabyAds.AdFalcon.Services.Interfaces.DTOs.Reports.Performance
{
    [ProtoContract]
    public class AccountCriteria
    {
       [ProtoMember(1)]
        public string Email { get; set; }

       [ProtoMember(2)]
        public string CompanyName { get; set; }

       [ProtoMember(3)]
        public string Name { get; set; }

    }
}
