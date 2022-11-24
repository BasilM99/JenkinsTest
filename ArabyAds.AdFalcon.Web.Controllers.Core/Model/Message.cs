using ArabyAds.AdFalcon.Web.Controllers.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace ArabyAds.AdFalcon.Web.Controllers.Model
{

    public enum ResponseStatus { success, businessException,redirect }
    public class ResultMessage
    {

        public string Message { get; set; }
        public MessagesType Type { get; set; }

        
    }
}
