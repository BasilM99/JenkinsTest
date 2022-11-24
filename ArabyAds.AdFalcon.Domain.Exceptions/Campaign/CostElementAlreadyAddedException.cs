using System;
using System.Collections.Generic;
using ArabyAds.Framework.ExceptionHandling.Exceptions;

namespace ArabyAds.AdFalcon.Exceptions.Campaign
{
    /// <summary>
    /// This exception can be thrown when Cost Element already added to group
    /// </summary>
    public class CostElementAlreadyAddedException : BusinessException
    {
        public CostElementAlreadyAddedException(IList<ErrorData> errors = null, Exception innerException = null, string message = null)
            : base(errors, innerException, message)
        {
            this.Errors = new List<ErrorData>()
                              {
                                  new ErrorData(message:Framework.Resources.ResourceManager.Instance.GetResource("CostElementAlreadyAddedBR", "Global"))
                              };
        }

    }
    public class CostElementPercentageAddedException : BusinessException
    {
        public CostElementPercentageAddedException(IList<ErrorData> errors = null, Exception innerException = null, string message = null)
            : base(errors, innerException, message)
        {
            this.Errors = new List<ErrorData>()
                              {
                                  new ErrorData(message:Framework.Resources.ResourceManager.Instance.GetResource("CostElementSumAddedBR", "Global"))
                              };
        }

    }
    public class FeeAlreadyAddedException : BusinessException
    {
        public FeeAlreadyAddedException(IList<ErrorData> errors = null, Exception innerException = null, string message = null)
            : base(errors, innerException, message)
        {
            this.Errors = new List<ErrorData>()
                              {
                                  new ErrorData(message:Framework.Resources.ResourceManager.Instance.GetResource("FeeAlreadyAddedBR", "Global"))
                              };
        }

    }

    public class AdGroupDynamicBiddingConfigAlreadyAddedException : BusinessException
    {
        public AdGroupDynamicBiddingConfigAlreadyAddedException(IList<ErrorData> errors = null, Exception innerException = null, string message = null)
            : base(errors, innerException, message)
        {
            this.Errors = new List<ErrorData>()
                              {
                                  new ErrorData(message:Framework.Resources.ResourceManager.Instance.GetResource("AdGroupDynamicBiddingConfigAlreadyAddedBR", "Global"))
                              };
        }

    }


}
