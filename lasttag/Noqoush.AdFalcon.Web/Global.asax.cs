using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;

using Noqoush.AdFalcon.Web.Controllers.Controllers;
using Noqoush.AdFalcon.Web.Controllers.Core;
using Noqoush.AdFalcon.Web.Controllers.Utilities;
using Noqoush.Framework;
using Noqoush.Framework.Persistence;
using Noqoush.Framework.Security;
using Noqoush.Framework.Web.ClientValidation;
using System.Threading;
using System.Globalization;

namespace Noqoush.AdFalcon.Web
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : WebApplicationBase
    {

        protected void Application_Error(object sender, EventArgs e)
        {
           
            //var ex = Server.GetLastError();
            //ApplicationContext.Instance.Logger.Error(ex.Message, ex);

        }
    }


    
}