using Fasterflect;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Newtonsoft.Json.Linq;
using ArabyAds.AdFalcon.API.Controllers.Core.ExceptionHandling;
using ArabyAds.AdFalcon.API.Controllers.Utilities;
using ArabyAds.AdFalcon.Services.Interfaces.Services.Account;
using ArabyAds.Framework;
using ArabyAds.Framework.Caching;
using ArabyAds.Framework.ExceptionHandling.Exceptions;
using ArabyAds.Framework.Security;
using ArabyAds.Framework.Utilities;
using Org.BouncyCastle.Asn1.X509.Qualified;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using ArabyAds.AdFalcon.Services.Interfaces.Services.Core;

namespace ArabyAds.AdFalcon.API.Controllers.Core.Controllers
{
    public class ControllerBase : Controller
    {
         static SecurityManager _securityManager;
        static IAccountService _accountService;

        static ControllerBase()
        {
            _securityManager = new SecurityManager(IoC.Instance.Resolve<ISecurityService>());
            _accountService = IoC.Instance.Resolve<IAccountService>();
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
            RouteData routeData = GetRouteData(context.HttpContext);

            if (routeData.Values["controller"].ToString().ToLower() == "test")
            {
                return;
            }
            string Domain = "adfalcon.com";
        

        var tenantDto = IoC.Instance.Resolve<ITenantService>().GetTenantByDomain(Domain);
            ApplicationContext.CreateContext(tenantDto.Name, false, new Tenant { Name = tenantDto.Name, ID = tenantDto.ID, Domain = tenantDto.Domain, Code = tenantDto.Code });
  
            string clientId = BuildSecurityContext(routeData);

            //Check request parameters and get account id
            int accountId = CheckRequestParameters(clientId, routeData, context);

            // Validate version number for this request
            ValidateVersionNumber(routeData);

            //Validate quota
            ValidateClientQuota(clientId);

            SetAccountIdInRouteData(accountId);
        }
       
        private RouteData GetRouteData(HttpContext context)
        {
            var routeData = context.GetRouteData();

            if (routeData == null)
            {
                throw new APIException(ErrorCode.BadRequest, APIExceptionMessages.MissingParameters);
            }

            return routeData;
        }

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

        private int CheckRequestParameters(string clientId, RouteData routeData, ActionExecutingContext context)
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
                            ValidateRequestParameters(clientId, accountAccessDto.APISecretKey, urlHashedParameter.ToString(), context);

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
                catch (BusinessException)
                {
                    throw new APIException(ErrorCode.BadRequest, APIExceptionMessages.NowAllowedAccess);
                }
            }
            else
            {
                throw new APIException(ErrorCode.BadRequest, APIExceptionMessages.ClientIdIsNotProvided);
            }

        }

        private void ValidateRequestParameters(string clientId, string apiSecretKey, string urlHashedParameter, ActionExecutingContext context)
        {
          
            SortedDictionary<string, string> parameters = GetCriteriaParameters(context);

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

        private static ConcurrentDictionary<Type, IEnumerable<string>> _criteriaTypeProperties = new ConcurrentDictionary<Type, IEnumerable<string>>();
        private SortedDictionary<string, string> GetCriteriaParameters(ActionExecutingContext context)
        {
            SortedDictionary<string, string> parameters = new SortedDictionary<string, string>();
            object container = null;
            if (!context.ActionArguments.TryGetValue("container", out  container))
                      return parameters;
            object criteria = container.TryGetValue("criteria");
            if (criteria == null)
                return parameters;
            var criteriaType = criteria.GetType();
            if (!_criteriaTypeProperties.TryGetValue(criteriaType, out var props))
            {
                props = criteriaType.GetProperties().Select(x => x.Name);
                _criteriaTypeProperties.TryAdd(criteria.GetType(), props);
            }
            foreach (var prop in props)
            {
                var valueprop = criteria.GetPropertyValue(prop);
                string valuepropstr = null;
                if (valueprop == null)
                {
                    valuepropstr = string.Empty;
                }
                else
                {
                    valuepropstr = valueprop.ToString();
                }
                if (prop.ToLower()=="istest")
                { 
                    if (valuepropstr.ToLower()=="false")
                    {
                        valuepropstr = null;
                    }
                
                }
                if(!string.IsNullOrEmpty(valuepropstr))
                parameters.Add(prop.ToLower(), criteria.GetPropertyValue(prop)?.ToString()?? string.Empty);
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
    }
}
