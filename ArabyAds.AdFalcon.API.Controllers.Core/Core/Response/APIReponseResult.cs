using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ArabyAds.AdFalcon.API.Controllers.Core.Response.ResponseData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


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

        public override void ExecuteResult(ActionContext context)
        {
          
            
            SendResult(context);
        }

        private void SendResult(ActionContext context )
        {
            SetRequestHeaders(context);
            SetOutputResult(context);
        }

        private void SetOutputResult(ActionContext context)
        {
            string dataToOutput = _responseData.GetReponseString();
            APIResponseWriter.WriteResponse(dataToOutput,true, context);
        }

        private void SetRequestHeaders(ActionContext context)
        {
            var response = context.HttpContext.Response;

            response.ContentType = _responseData.ResponseHeader;
        }
    }
}
