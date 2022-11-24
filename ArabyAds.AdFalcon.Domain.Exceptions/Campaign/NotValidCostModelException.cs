using System;
using System.Collections.Generic;
using ArabyAds.Framework.ExceptionHandling.Exceptions;

namespace ArabyAds.AdFalcon.Exceptions.Campaign
{
    /// <summary>
    /// This exception can be thrown when Cost Model in valid
    /// </summary>
    public class NotValidCostModelException : BusinessException
    {
        public NotValidCostModelException(IList<ErrorData> errors = null, Exception innerException = null, string message = null)
            : base(errors, innerException, message)
        {
            this.Errors = new List<ErrorData>()
                              {
                                  new ErrorData(message:Framework.Resources.ResourceManager.Instance.GetResource("NotValidCostModelBR", "Global"))
                              };
        }

    }



    /// <summary>
    /// This exception can be thrown when Cost Model in not selected in targeting
    /// </summary>
    public class SelectCostModelException : BusinessException
    {
        public SelectCostModelException(IList<ErrorData> errors = null, Exception innerException = null, string message = null)
            : base(errors, innerException, message)
        {
            this.Errors = new List<ErrorData>()
                              {
                                  new ErrorData(message:Framework.Resources.ResourceManager.Instance.GetResource("NotValidCostModelBR", "Global"))
                              };
        }

    }
}
