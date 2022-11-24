using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Noqoush.AdFalcon.Services.Interfaces.DTOs.Core
{

    public enum Exceptions
    {
        NotAuthorizedException = 0,
        NotAuthenticatedException = 1,
        WrappedException = 2,
        BusinessException = 3
    }

    [DataContract]
    public class ExceptionDto
    {
        [DataMember]
        public Exceptions type { get; set; }
        [DataMember]
        public string Message { get; set; }
    }
}
