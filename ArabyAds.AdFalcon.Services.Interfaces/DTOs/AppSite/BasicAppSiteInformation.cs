using System;
using System.Collections.Generic;
using System.Linq;
using ProtoBuf;
using System.Text;
using System.Threading.Tasks;

namespace ArabyAds.AdFalcon.Services.Interfaces.DTOs.AppSite
{
    [ProtoContract]
    public class BasicAppSiteInformation
    {
       [ProtoMember(1)]
        public string EmailAddress { get; set; }

       [ProtoMember(2)]
        public string AccountName { get; set; }

       [ProtoMember(3)]
        public string AppsiteUrl { get; set; }

    }
}
