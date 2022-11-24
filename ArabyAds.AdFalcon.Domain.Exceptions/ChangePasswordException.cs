using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArabyAds.Framework.ExceptionHandling.Exceptions;

namespace ArabyAds.AdFalcon.Business.Domain.Exceptions
{
    public class ChangePasswordException : BusinessException
    {
        public ChangePasswordException(IList<ErrorData> errors = null, Exception innerException = null, string message = null)
            : base(errors, innerException,message)
        {

        }

    }
}
