using System;
using System.Collections.Generic;
using ArabyAds.Framework.ExceptionHandling.Exceptions;

namespace ArabyAds.AdFalcon.Exceptions.Fund
{
    public class TransactionAmountExpectation : BusinessException
    {
        public TransactionAmountExpectation(IList<ErrorData> errors = null, Exception innerException = null, string message = null)
            : base(errors, innerException, message)
        {
        }
    }
}
