using System;
using System.Collections.Generic;
using Noqoush.Framework.ExceptionHandling.Exceptions;

namespace Noqoush.AdFalcon.Exceptions.Core
{
    public class NoChangesException : BusinessException
    {
        public NoChangesException(IList<ErrorData> errors = null, Exception innerException = null, string message = null)
            : base(errors, innerException, message)
        {
            this.Errors = new List<ErrorData>()
                              {
                                  new ErrorData(message:Framework.Resources.ResourceManager.Instance.GetResource("NoChangesBR", "Global"))
                              };
        }
    }
}