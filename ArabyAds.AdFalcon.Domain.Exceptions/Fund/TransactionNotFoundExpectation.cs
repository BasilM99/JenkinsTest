using System;
using System.Collections.Generic;
using ArabyAds.Framework.ExceptionHandling.Exceptions;

namespace ArabyAds.AdFalcon.Exceptions.Fund
{
    public class TransactionNotFoundExpectation : BusinessException
    {
        public TransactionNotFoundExpectation(IList<ErrorData> errors = null, Exception innerException = null, string message = null)
            : base(errors, innerException, message)
        {
        }
    }
}
