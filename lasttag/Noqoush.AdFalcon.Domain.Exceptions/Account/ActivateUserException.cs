using System;
using System.Collections.Generic;
using Noqoush.Framework.ExceptionHandling.Exceptions;

namespace Noqoush.AdFalcon.Exceptions.Account
{
    /// <summary>
    /// This exception can be thrown when the user activation process failed
    /// </summary>
    public class NotActivatedUserException : BusinessException
    {
        public NotActivatedUserException(IList<ErrorData> errors = null, Exception innerException = null, string message = null)
            : base(errors, innerException, message)
        {

        }

    }
}
