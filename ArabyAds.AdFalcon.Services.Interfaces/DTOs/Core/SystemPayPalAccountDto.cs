using System;
using System.Collections.Generic;
using System.Text;
//using Gallio.Framework;
//using MbUnit.Framework;
//using MbUnit.Framework.ContractVerifiers;
using ProtoBuf;
using ArabyAds.Framework.DataAnnotations;

namespace ArabyAds.AdFalcon.Services.Interfaces.DTOs.Core
{
    [ProtoContract]
    public class SystemPayPalAccountDto
    {
       [ProtoMember(1)]
        public int ID { get; set; }

       [ProtoMember(2)]
        public string UserName{get;set;}
    }
}
