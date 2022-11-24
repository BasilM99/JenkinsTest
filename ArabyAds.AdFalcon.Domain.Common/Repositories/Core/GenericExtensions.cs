using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Collections.Specialized;

using System.Web;
//namespace ArabyAds.AdFalcon.Domain.Common.Repositories.Core
//{
    public static  class GenericExtensions
    {
        public static object GetPropertyValue(this object ReportItem, string propertyName)
        {
            return ReportItem.GetType().GetProperties()
               .Single(pi => pi.Name == propertyName)
               .GetValue(ReportItem, null);
        }

        public static void SetPropertyValue(this object ReportItem, object Value , string propertyName)
        {
             ReportItem.GetType().GetProperties()
               .Single(pi => pi.Name == propertyName)
               .SetValue(ReportItem, Value);
        }

        public static string AddGetParamToUrl(string url, string pname, string pvalue)
        {

            pvalue = Uri.EscapeDataString(pvalue);

            if (url.IndexOf("?") > -1)
            {

                url = url.Replace("?", string.Format("?{0}={1}&", pname, pvalue));

            }
                        else
            {

                url = string.Format("{0}?{1}={2}", url, pname, pvalue);

            }

            return url;

        }
    }




public static class StringExtensions
    {
        public static string AddToQueryString(this string url, params object[] keysAndValues)
        {
            return UpdateQueryString(url, q =>
            {
                for (var i = 0; i < keysAndValues.Length; i += 2)
                {
                    q.Set(keysAndValues[i].ToString(), keysAndValues[i + 1].ToString());
                }
            });
        }

        public static string RemoveFromQueryString(this string url, params string[] keys)
        {
            return UpdateQueryString(url, q =>
            {
                foreach (var key in keys)
                {
                    q.Remove(key);
                }
            });
        }

        public static string UpdateQueryString(string url, Action<NameValueCollection> func)
        {
            var urlWithoutQueryString = url.Contains('?') ? url.Substring(0, url.IndexOf('?')) : url;
            var queryString = url.Contains('?') ? url.Substring(url.IndexOf('?')) : null;
            //var query = HttpUtility.ParseQueryString(queryString ?? string.Empty);
            queryString= queryString ?? string.Empty;
            NameValueCollection query = new NameValueCollection();
            string[] querySegments = queryString.Split('&');
            foreach (string segment in querySegments)
            {
                string[] parts = segment.Split(new char[] { '='},2);
                if (parts.Length >1 )
                {
                    string key = parts[0].Trim(new char[] { '?', ' ' });
                    string val = parts[1].Trim();

                    query.Add(key, val);
                }
            }


            func(query);

            return urlWithoutQueryString + (query.Count > 0 ? "?" : string.Empty) + string.Join("&",
            query
                   .AllKeys
                   .Select(key => key + "=" + query[key])
      .ToArray());
        }
    }
//}
