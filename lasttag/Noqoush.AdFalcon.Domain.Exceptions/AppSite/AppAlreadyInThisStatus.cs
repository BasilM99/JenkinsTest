using System;
using System.Collections.Generic;
using Noqoush.Framework.ExceptionHandling.Exceptions;

namespace Noqoush.AdFalcon.Exceptions.AppSite
{
    public class AppAlreadyInThisStatus : BusinessException
    {
        public AppAlreadyInThisStatus(IList<ErrorData> errors = null, Exception innerException = null, string message = null)
            : base(errors, innerException, message)
        {
            Errors = new List<ErrorData>() { new ErrorData() { ID = "AppAlreadyInThisStatus" } };
        }
    }
}
