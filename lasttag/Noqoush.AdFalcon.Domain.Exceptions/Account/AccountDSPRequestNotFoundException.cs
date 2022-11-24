

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Noqoush.Framework.ExceptionHandling.Exceptions;

namespace Noqoush.AdFalcon.Exceptions.Core
{
    public class AccountDSPRequestNotFoundException : BusinessException
    {
        public AccountDSPRequestNotFoundException(IList<ErrorData> errors = null, Exception innerException = null, string message = null)
            : base(errors, innerException, message)
        {
        }
    }
}
