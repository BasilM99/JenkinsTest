using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProtoBuf;

namespace ArabyAds.AdFalcon.Services.Interfaces.DTOs.Core
{

    public enum Exceptions
    {
        NotAuthorizedException = 0,
        NotAuthenticatedException = 1,
        WrappedException = 2,
        BusinessException = 3
    }

    [ProtoContract]
    public class ExceptionDto
    {
       [ProtoMember(1)]
        public Exceptions type { get; set; }
       [ProtoMember(2)]
        public string Message { get; set; }
    }
}
