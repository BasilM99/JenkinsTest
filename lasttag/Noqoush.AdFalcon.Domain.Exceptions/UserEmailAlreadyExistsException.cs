using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Noqoush.Framework.ExceptionHandling.Exceptions;

namespace Noqoush.AdFalcon.Business.Domain.Exceptions
{
    /// <summary>
    /// This expception can be thrown when the user email is duplicated
    /// </summary>
    public class UserEmailAlreadyExistsException : BusinessException
    {
        public UserEmailAlreadyExistsException(IList<ErrorData> errors = null, Exception innerException = null, string message = null)
            : base(errors, innerException,message)
        {

        }

    }
}
