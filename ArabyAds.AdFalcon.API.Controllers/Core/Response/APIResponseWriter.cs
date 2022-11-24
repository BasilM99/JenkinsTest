
using ArabyAds.AdFalcon.API.Controllers.Core.ExceptionHandling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace ArabyAds.AdFalcon.API.Controllers.Core.Response
{
    /// <summary>
    /// Write the api response to the response stream with the proper http headers
    /// </summary>
    public static class APIResponseWriter
    {
        /// <summary>
        /// Writes data to response stream  with "success" value for "X-AdFalconAPI-Status" header
        /// </summary>
        /// <param name="data"></param>
        public static void WriteResponse(string data, bool endResponse)
        {
            HttpResponseBase response = new HttpResponseWrapper(HttpContext.Current.Response);
            response.AddHeader("X-AdFalconAPI-Status", "success");
            response.CacheControl = "no-cache";
            response.ClearContent();
            response.BufferOutput = false;
            response.Write(data);

            if (endResponse) { response.End(); }
        }

        /// <summary>
        /// Writes the exception details in response http headers ("X-AdFalconAPI-Errorcode","X-AdFalconAPI-Errordetails") , writes "error" for "X-AdFalconAPI-Status" header.
        /// </summary>
        /// <param name="exception"></param>
        public static void WriteErrorResponse(APIException exception,bool endResponse)
        {
            HttpResponseBase response = new HttpResponseWrapper(HttpContext.Current.Response);
            response.CacheControl = "no-cache";
            response.AddHeader("X-AdFalconAPI-Status", "error");
            response.AddHeader("X-AdFalconAPI-ErrorCode", exception.Code.ToString());
            response.AddHeader("X-AdFalconAPI-ErrorDetails", exception.Message);

            if (endResponse) { response.End(); }
        }
    }
}
