using System;
using System.Collections.Generic;
using System.Linq;
using ProtoBuf;
using System.Text;
using System.Threading.Tasks;

namespace ArabyAds.AdFalcon.Services.Interfaces.DTOs.Account
{
    [ProtoContract]

    public class AccountAdPermissionsDto
    {

       [ProtoMember(1)]
        public virtual int AccountId { get; set; }
       [ProtoMember(2)]
        public virtual string GivenPermissionAdCodes { get; set; }


    }
}
