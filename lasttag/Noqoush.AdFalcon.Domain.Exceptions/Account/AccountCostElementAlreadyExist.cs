
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Noqoush.Framework.ExceptionHandling.Exceptions;

namespace Noqoush.AdFalcon.Exceptions.Core
{
    public class AccountCostElementAlreadyExist : BusinessException
    {
        public AccountCostElementAlreadyExist(IList<ErrorData> errors = null, Exception innerException = null, string message = null)
            : base(errors, innerException, message)
        {
        }
    }
    public class AccountCostElementNoDataProvider : BusinessException
    {
        public AccountCostElementNoDataProvider(IList<ErrorData> errors = null, Exception innerException = null, string message = null)
            : base(errors, innerException, message)
        {
        }
    }

    public class AccountFeeAlreadyExist : BusinessException
    {
        public AccountFeeAlreadyExist(IList<ErrorData> errors = null, Exception innerException = null, string message = null)
            : base(errors, innerException, message)
        {
        }
    }
}
