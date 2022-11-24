using Newtonsoft.Json;
using ArabyAds.AdFalcon.API.Controllers.Core.Response.ResponseData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ArabyAds.AdFalcon.API.Controllers.Core.Response
{
    public class APIReponseResult<T> : ActionResult
        where T : class
    {
        private IAPIResponseData<T> _responseData;

        public APIReponseResult(IAPIResponseData<T> responseData)
        {
            this._responseData = responseData;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            SendResult();
        }

        private void SendResult()
        {
            SetRequestHeaders();
            SetOutputResult();
        }

        private void SetOutputResult()
        {
            string dataToOutput = _responseData.GetReponseString();
            APIResponseWriter.WriteResponse(dataToOutput,true);
        }

        private void SetRequestHeaders()
        {
            var response = HttpContext.Current.Response;

            response.ContentType = _responseData.ResponseHeader;
        }
    }
}
