using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Noqoush.AdFalcon.Services.Interfaces.DTOs.Account
{
    [DataContract]

    public class AccountAdPermissionsDto
    {

        [DataMember]
        public virtual int AccountId { get; set; }
        [DataMember]
        public virtual string GivenPermissionAdCodes { get; set; }


    }
}
