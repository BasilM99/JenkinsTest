using System;
using System.Collections.Generic;
using ArabyAds.Framework.ExceptionHandling.Exceptions;

namespace ArabyAds.AdFalcon.Exceptions.AppSite
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
