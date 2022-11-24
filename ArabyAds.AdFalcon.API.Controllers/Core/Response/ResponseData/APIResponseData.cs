using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArabyAds.AdFalcon.API.Controllers.Core.Response.ResponseData
{
    public interface IAPIResponseData<T>
    {
        T Data { get; set; }

        string ResponseHeader { get;  }

        string GetReponseString();
    }
}
