using System;
using System.Collections.Generic;
using ArabyAds.Framework.ExceptionHandling.Exceptions;

namespace ArabyAds.AdFalcon.Exceptions.Fund
{
    public class TransactionAlreadyClosedExpectation : BusinessException
    {
        public TransactionAlreadyClosedExpectation(IList<ErrorData> errors = null, Exception innerException = null, string message = null)
            : base(errors, innerException, message)
        {
        }
    }
}
