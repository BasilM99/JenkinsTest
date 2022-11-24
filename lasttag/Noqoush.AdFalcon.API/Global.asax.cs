using Newtonsoft.Json.Linq;
using Noqoush.AdFalcon.API.Controllers.Core;
using Noqoush.AdFalcon.API.Controllers.Core.ExceptionHandling;
using Noqoush.AdFalcon.API.Controllers.Core.Response;
using Noqoush.AdFalcon.API.Controllers.Mapping;
using Noqoush.AdFalcon.API.Controllers.Utilities;
using Noqoush.AdFalcon.API.Core;
using Noqoush.AdFalcon.API.Core.Routing;
using Noqoush.AdFalcon.Services.Interfaces.Services.Account;
using Noqoush.Framework;
using Noqoush.Framework.Caching;
using Noqoush.Framework.ConfigurationSetting;
using Noqoush.Framework.ExceptionHandling.Exceptions;
using Noqoush.Framework.Security;
using Noqoush.Framework.Storage;
using Noqoush.Framework.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Script.Serialization;

namespace Noqoush.AdFalcon.API
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class APIApplication : System.Web.HttpApplication
    {
        protected DefaultControllerFactory _controllerFactory;
        private IAccountService _accountService = IoC.Instance.Resolve<IAccountService>();
        
        static private SecurityManager _securityManager;

        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            //This line has been removed intentionally to force all logs to go through Application_Error event in Global.asax file
            //filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.Add("APIRoute", new APIRoute(Config.APIDomainFormat, "{version}/{clientId}/{hash}/{controller}/{action}", new { IsTest = false }));
            routes.Add("TestAPIRoute", new APIRoute(Config.APIDomainFormat, "test/{version}/{clientId}/{hash}/{controller}/{action}", new { IsTest = true }));
            routes.Add("Default", new APIRoute(Config.APIDomainFormat, "{controller}/{action}", null));
        }

        protected void Application_Start()
        {
            RegisterControllerFactory();
            RegisterSecurityManager();

            AreaRegistration.RegisterAllAreas();
            
            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
            RegisterMapping();
        }

        
        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {
            RouteData routeData = GetRouteData();

            if (routeData.Values["controller"].ToString().ToLower() == "test")
            {
                return;
            }

            string clientId = BuildSecurityContext(routeData);

            //Check request parameters and get account id
            int accountId = CheckRequestParameters(clientId, routeData);

            // Validate version number for this request
            ValidateVersionNumber(routeData);

            //Validate quota
            ValidateClientQuota(clientId);

            SetAccountIdInRouteData(accountId);
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            var ex = Server.GetLastError();

            if (ex is APIException)
            {
                APIResponseWriter.WriteErrorResponse(ex as APIException, true);
                Server.ClearError();
            }
            else
            {
                APIException exception = new APIException(ErrorCode.InternalServerError,APIExceptionMessages.InternalServerError);
                APIResponseWriter.WriteErrorResponse(exception, true);

                Noqoush.Framework.ExceptionHandling.ExceptionPolicy.HandleException(ex, Noqoush.Framework.ExceptionHandling.ExceptionHandlingPolicies.UI);

                Server.ClearError();

            }
        }

        #region Private Members

        private void RegisterControllerFactory()
        {
            List<Assembly> controllerAssemblies = new List<Assembly>();
            controllerAssemblies.Add(Assembly.Load("Noqoush.AdFalcon.API.Controllers"));

            _controllerFactory = new NoqoushControllerFactory(controllerAssemblies);
            ControllerBuilder.Current.SetControllerFactory(_controllerFactory);
        }

        private void RegisterSecurityManager()
        {
            _securityManager = new SecurityManager(IoC.Instance.Resolve<ISecurityService>());
        }

        private void RegisterMapping()
        {
            MappingRegister.RegisterMapping();
        }

        /// <summary>
        /// Get <seealso cref="RouteData"/> object for the current request
        /// </summary>
        /// <returns></returns>
        private RouteData GetRouteData()
        {
            var routeData = RouteTable.Routes.GetRouteData(new HttpContextWrapper(Context));

            if (routeData == null)
            {
                throw new APIException(ErrorCode.BadRequest, APIExceptionMessages.MissingParameters);
            }

            return routeData;
        }

        /// <summary>
        /// Build security context for the current request
        /// </summary>
        /// <returns></returns>
        private string BuildSecurityContext(RouteData routeData)
        {
            _securityManager.BuildSecurityContext(string.Empty);

            var clientId = routeData.Values["clientId"];

            if (clientId != null)
            {
                return clientId.ToString();
            }

            else
            {
                throw new APIException(ErrorCode.BadRequest, APIExceptionMessages.ClientIdIsNotProvided);
            }

        }

        /// <summary>
        /// Check that request parameters are valid and associated with account in our system
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="routeData"></param>
        private int CheckRequestParameters(string clientId, RouteData routeData)
        {
            if (clientId != null)
            {
                //Get the associated account and secret key with this clientId
                try
                {
                    var accountAccessDto = _accountService.GetAccountAPIAccessByAPIClientId(clientId.ToString());

                    if (accountAccessDto != null)
                    {
                        var urlHashedParameter = routeData.Values["hash"];

                        if (urlHashedParameter != null)
                        {
                            ValidateRequestParameters(clientId, accountAccessDto.APISecretKey, urlHashedParameter.ToString());

                            return accountAccessDto.AccountId;
                        }
                        else
                        {
                            throw new APIException(ErrorCode.BadRequest, APIExceptionMessages.HashIsNotProvided);
                        }

                    }
                    else
                    {
                        throw new APIException(ErrorCode.Unauthorized, APIExceptionMessages.ClientIdIsNotValid);
                    }
                }
                catch (BusinessException x)
                {
                    throw new APIException(ErrorCode.BadRequest, APIExceptionMessages.NowAllowedAccess);
                }
            }
            else
            {
                throw new APIException(ErrorCode.BadRequest, APIExceptionMessages.ClientIdIsNotProvided);
            }

        }

        private void ValidateRequestParameters(string clientId, string apiSecretKey, string urlHashedParameter)
        {
            NameValueCollection parametersCollection = ConvertJSONParametersToNameValueCollection(Request.InputStream);
            SortedDictionary<string, string> parameters = GetParameters(parametersCollection);
            
            //concat parameters into one string
            var parametersString = parameters.Aggregate(string.Empty,
                                                 (current, parameter) =>
                                                 current +
                                                 string.Format("{0}={1}&",
                                                 parameter.Key.Trim(),
                                                 parameter.Value.Trim())
                                                     .Trim());

            // Remove the last "&" at the end of the string

            if (!string.IsNullOrEmpty(parametersString))
            {
                parametersString = parametersString.Substring(0, parametersString.Length - 1);
                parametersString = parametersString.Insert(0, string.Format("ckey={0}&", clientId));

                //generate the parameters hash using the API key
                var hash = Hashing.GenraterHMAC(apiSecretKey, parametersString);

                if (hash != urlHashedParameter)
                {
                    throw new APIException(ErrorCode.Unauthorized, APIExceptionMessages.HashIsTampered);
                }
            }
            else
            {
                throw new APIException(ErrorCode.BadRequest, APIExceptionMessages.MissingParameters);
            }

        }

        private NameValueCollection ConvertJSONParametersToNameValueCollection(Stream stream)
        {
            NameValueCollection jsonParametersNameValue = new NameValueCollection();

            string jsonParametersString = GetJSONString(stream);
           
            if (!string.IsNullOrEmpty(jsonParametersString))
            {
                JObject jsonParameters = JObject.Parse(Server.UrlDecode(jsonParametersString));

                if (jsonParameters["criteria"] != null)
                {
                    foreach (var result in jsonParameters["criteria"])
                    {
                        jsonParametersNameValue.Add(((JProperty)result).Name, ((JProperty)result).Value.ToString());
                    }
                }
            }

            return jsonParametersNameValue;
        }

        private string GetJSONString(Stream stream)
        {
            string jsonParametersString = null;
            using (MemoryStream memoeryRequestStream = new MemoryStream())
            {
                stream.CopyTo(memoeryRequestStream, (int)stream.Length);

                memoeryRequestStream.Seek(0, SeekOrigin.Begin);
                using (StreamReader requestStreamReader = new StreamReader(memoeryRequestStream))
                {
                    jsonParametersString = requestStreamReader.ReadToEnd();
                }

            }

            return jsonParametersString;
        }

        /// <summary>
        /// Convert <seealso cref="NameValueCollection"/> to <seealso cref="SortedDictionary"/>
        /// </summary>
        /// <param name="nameValueCollection"></param>
        /// <returns></returns>
        private SortedDictionary<string, string> GetParameters(NameValueCollection nameValueCollection)
        {
            SortedDictionary<string, string> parameters = new SortedDictionary<string, string>();

            foreach (var item in nameValueCollection)
            {
                parameters.Add(item.ToString(), nameValueCollection[item.ToString()] == null ? string.Empty : nameValueCollection[item.ToString()].ToString());
            }

            return parameters;
        }


        /// <summary>
        /// Check if version number is supported
        /// </summary>
        private void ValidateVersionNumber(RouteData routeData)
        {
            var versionNumber = routeData.Values["version"];

            if (versionNumber != null && !string.IsNullOrEmpty(versionNumber.ToString()))
            {
                string fixedVersionNumber = versionNumber.ToString().ToLower();

                bool isSupported = Config.SupportedAPIVersionNumbers.Any(p => p.ToLower() == fixedVersionNumber);

                if (!isSupported)
                {
                    throw new APIException(ErrorCode.BadRequest, APIExceptionMessages.VersionNotSupported);
                }

            }
            else
            {
                throw new APIException(ErrorCode.BadRequest, APIExceptionMessages.VersionIsNotProvided);
            }
        }

        private void SetAccountIdInRouteData(int accountId)
        {
            CallContext.Current.Items["AccountId"] = accountId;
        }

        private void ValidateClientQuota(string clientId)
        {
            int maxRequestsPerMinute = Config.MaxRequestPerMinute;
            int maxRequestsPerDay = Config.MaxRequestPerDay;

            // Get cache key names
            string lastRequestCacheKey = GetLastRequestCacheKey(clientId);
            string cacheKeyNameForMinutesCounter = GetCacheKeyForMinutesCounter(clientId);
            string cacheKeyNameForDaysCounter = GetCacheKeyForHoursCounter(clientId);

            DateTime? lastRequestDateTime = CacheManager.Current.DefaultProvider.Get<DateTime?>(lastRequestCacheKey);
            int minutesCounter = CacheManager.Current.DefaultProvider.Get<int>(cacheKeyNameForMinutesCounter);
            int daysCounter = CacheManager.Current.DefaultProvider.Get<int>(cacheKeyNameForDaysCounter);

            //If this is the first request, set all counters to (one)
            if (!lastRequestDateTime.HasValue)
            {
                CacheManager.Current.DefaultProvider.Put<int>(cacheKeyNameForMinutesCounter, 1);
                CacheManager.Current.DefaultProvider.Put<int>(cacheKeyNameForDaysCounter, 1);
            }
            else
            {
                if (((DateTime.Now - lastRequestDateTime.Value).TotalMinutes) > 1)
                {
                    CacheManager.Current.DefaultProvider.Put<int>(cacheKeyNameForMinutesCounter, 1);
                }
                else
                {
                    if (minutesCounter++ > maxRequestsPerMinute)
                    {
                        throw new APIException(ErrorCode.ServiceUnavailable, APIExceptionMessages.ExceedsQuota);
                    }
                    else
                    {
                        CacheManager.Current.DefaultProvider.Put<int>(cacheKeyNameForMinutesCounter, minutesCounter);
                    }
                }


                if (((DateTime.Now - lastRequestDateTime.Value).TotalDays) > 1)
                {
                    CacheManager.Current.DefaultProvider.Put<int>(cacheKeyNameForDaysCounter, 1);
                }
                else
                {
                    if (daysCounter++ > maxRequestsPerDay)
                    {
                        throw new APIException(ErrorCode.ServiceUnavailable, APIExceptionMessages.ExceedsQuota);
                    }
                    else
                    {
                        CacheManager.Current.DefaultProvider.Put<int>(cacheKeyNameForDaysCounter, daysCounter);
                    }
                }
            }

            //Save last time for client request
            CacheManager.Current.DefaultProvider.Put<DateTime?>(lastRequestCacheKey, DateTime.Now);

        }

        private string GetCacheKeyForMinutesCounter(string clientId)
        {
            return string.Format("MinutesRequestsCounter-{0}", clientId);
        }

        private string GetCacheKeyForHoursCounter(string clientId)
        {
            return string.Format("HoursRequestsCounter-{0}", clientId);
        }

        private string GetLastRequestCacheKey(string clientId)
        {
            return string.Format("ClientLastRequestCall-{0}", clientId);
        }

        #endregion
    }
}