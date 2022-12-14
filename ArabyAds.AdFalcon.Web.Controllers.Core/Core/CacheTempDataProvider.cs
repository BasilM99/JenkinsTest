using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace ArabyAds.AdFalcon.Web.Controllers.Core
{
    public class CacheTempDataProvider : ITempDataProvider
    {
        internal const string TempDataCacheKey = "__ControllerTempData";
        private string GetCacheKey()
        {
            return TempDataCacheKey + "_" + Framework.OperationContext.Current.CurrentPrincipal.Token;
        }

        public virtual IDictionary<string, object> LoadTempData(HttpContext controllerContext)
        {

            var cacheManager = Framework.Caching.CacheManager.Current.DefaultProvider;
            var tempDataDictionary = Framework.Caching.RedisCache.RedisCacheSerilizer.FormatterSurrogateDeSerialize(cacheManager.Get<byte[]>(GetCacheKey()))
                 as Dictionary<string, object>;
            if (tempDataDictionary != null)
            {
                // If we got it from Cache, remove it so that no other request gets it 
                cacheManager.Remove(GetCacheKey());
                return tempDataDictionary;
            }
            return new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
        }

        public virtual void SaveTempData(HttpContext controllerContext, IDictionary<string, object> values)
        {
            if (controllerContext == null)
            {
                throw new ArgumentNullException("controllerContext");
            }

            var cacheManager = Framework.Caching.CacheManager.Current.DefaultProvider;
            bool isDirty = (values != null && values.Count > 0);
            if (isDirty)
            {
                cacheManager.Put<byte[]>(GetCacheKey(), Framework.Caching.RedisCache.RedisCacheSerilizer.FormatterSurrogateSerialize(values),new TimeSpan(0,5,0));
            }
            else
            {
                var tempDataDictionary =
                    Framework.Caching.RedisCache.RedisCacheSerilizer.FormatterSurrogateDeSerialize(cacheManager.Get<byte[]>(GetCacheKey())) as Dictionary<string, object>;
                if (tempDataDictionary != null)
                {
                    // If we got it from Cache, remove it so that no other request gets it 
                    cacheManager.Remove(GetCacheKey());
                }
            }
        }
    }
}