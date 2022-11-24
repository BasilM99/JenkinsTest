using System;
using System.Collections.Generic;
using System.Text;
using Gallio.Framework;
using MbUnit.Framework;
using MbUnit.Framework.ContractVerifiers;
using System.Runtime.Serialization;
using Noqoush.Framework.DataAnnotations;

namespace Noqoush.AdFalcon.Services.Interfaces.DTOs.Core
{
    [DataContract]
    public class SystemPayPalAccountDto
    {
        [DataMember]
        public int ID { get; set; }

        [DataMember]
        public string UserName{get;set;}
    }
}
