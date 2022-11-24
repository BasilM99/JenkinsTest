using System;
using System.Collections.Generic;
using Noqoush.Framework.ExceptionHandling.Exceptions;

namespace Noqoush.AdFalcon.Exceptions.Campaign
{
    /// <summary>
    /// This exception can be thrown when attempting to update locked campaign
    /// </summary>
    public class CampaignLockedException : BusinessException
    {
        public CampaignLockedException(IList<ErrorData> errors = null, Exception innerException = null, string message = null)
            : base(errors, innerException, message)
        {
            this.Errors = new List<ErrorData>()
                              {
                                  new ErrorData(message:Framework.Resources.ResourceManager.Instance.GetResource("CampaignLockedBR", "Global"))
                              };
        }

    }


    /// </summary>
    public class CampaignReadOnlyException : BusinessException
    {
        public CampaignReadOnlyException(IList<ErrorData> errors = null, Exception innerException = null, string message = null)
            : base(errors, innerException, message)
        {
            this.Errors = new List<ErrorData>()
                              {
                                  new ErrorData(message:Framework.Resources.ResourceManager.Instance.GetResource("CampaignReadOnlyBR", "Global"))
                              };
        }

    }
}
