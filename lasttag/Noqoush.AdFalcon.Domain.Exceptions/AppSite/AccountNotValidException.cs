using System;
using System.Collections.Generic;
using Noqoush.Framework.ExceptionHandling.Exceptions;

namespace Noqoush.AdFalcon.Exceptions
{
    public class AccountNotValidException : BusinessException
    {
        public AccountNotValidException(IList<ErrorData> errors = null, Exception innerException = null, string message = null)
            : base(errors, innerException, message)
        {
        }
    }
}
