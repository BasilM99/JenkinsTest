using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Noqoush.Framework.ExceptionHandling.Exceptions;

namespace Noqoush.AdFalcon.Business.Domain.Exceptions
{
    /// <summary>
    /// This expception can be thrown when the user activation process failed
    /// </summary>
    public class ActivateUserException : BusinessException
    {
        public ActivateUserException(IList<ErrorData> errors = null, Exception innerException = null, string message = null)
            : base(errors, innerException,message)
        {

        }

    }
}
