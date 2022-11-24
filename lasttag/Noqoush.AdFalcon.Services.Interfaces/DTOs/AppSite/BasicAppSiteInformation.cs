using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Noqoush.AdFalcon.Services.Interfaces.DTOs.AppSite
{
    [DataContract]
    public class BasicAppSiteInformation
    {
        [DataMember]
        public string EmailAddress { get; set; }

        [DataMember]
        public string AccountName { get; set; }

        [DataMember]
        public string AppsiteUrl { get; set; }

    }
}
