using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArabyAds.AdFalcon.API.Controllers.Core.ExceptionHandling
{
    public  enum  ErrorCode : int
    {
        ServiceUnavailable = 503,
        InternalServerError = 500,
        Unauthorized = 401,
        BadRequest = 400
    }

    public class APIException : ApplicationException
    {
        public int Code { get; set; }

        #region Constructors
       
        /// <summary>
        /// Initializes a new instance of the <see cref="APIException"/> class with error message.
        /// </summary>
        /// <param name="message">error message.</param>
        public APIException(string message)
            : base(message)
        {
            Code = (int)ErrorCode.BadRequest;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="APIException"/> class with error message.
        /// </summary>
        /// <param name="message">error message.</param>
        public APIException(ErrorCode errorCode, string message)
            : base(message)
        {
            Code = (int)errorCode;
        }
       
        #endregion
    }
}
