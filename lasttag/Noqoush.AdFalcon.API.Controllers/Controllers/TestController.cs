using Noqoush.AdFalcon.API.Controllers.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Noqoush.AdFalcon.API.Controllers
{
    public class TestController : Controller
    {
        //
        // GET: /Test/
        public class TestCriteria
        {
            public string PublicKey { get; set; }
            public string Hashing { get; set; }
            public string F { get; set; }
            public string FDate { get; set; }
            public string TDate { get; set; }
            public string AId { get; set; }
            public string GB { get; set; }
            public string L { get; set; }
            public string OS { get; set; }
            public string CC { get; set; }
            public string IsTest { get; set; }
            public string IsIncludeEmpty { get; set; }
        }

        public TestController()
        {

        }

        public ActionResult TestStats()
        {
            return View();
        }

        [HttpPost]
        public ActionResult TestStats(TestCriteria criteria)
        {
            string url = string.Empty;

            if (!string.IsNullOrEmpty(criteria.IsTest))
            {
                url = "http://" + Config.APIDomainFormat.Replace("{apitype}", "pub") +"/test/v1.0/" + criteria.PublicKey + "/" + criteria.Hashing + "/report/stats";
            }
            else
            {
                url = "http://"  + Config.APIDomainFormat.Replace("{apitype}", "pub") + "/v1.0/" + criteria.PublicKey + "/" + criteria.Hashing + "/report/stats";
            }

            StringBuilder jsonBuilder = new StringBuilder();

            jsonBuilder.Append(string.Format("\"{0}\":\"{1}\",", "f", criteria.F));

            if (criteria.IsIncludeEmpty == "true" || !string.IsNullOrEmpty(criteria.FDate))
            {
                jsonBuilder.Append(string.Format("\"{0}\":\"{1}\",", "fdate", criteria.FDate == null ? string.Empty : criteria.FDate));
            }

            if (criteria.IsIncludeEmpty == "true" || !string.IsNullOrEmpty(criteria.TDate))
            {
                jsonBuilder.Append(string.Format("\"{0}\":\"{1}\",", "tdate", criteria.TDate == null ? string.Empty : criteria.TDate));
            }

            if (criteria.IsIncludeEmpty == "true" || !string.IsNullOrEmpty(criteria.GB))
            {
                jsonBuilder.Append(string.Format("\"{0}\":\"{1}\",", "gb", criteria.GB == null ? string.Empty : criteria.GB));
            }

            if (criteria.IsIncludeEmpty == "true" || !string.IsNullOrEmpty(criteria.AId))
            {
                jsonBuilder.Append(string.Format("\"{0}\":\"{1}\",", "aid", criteria.AId == null ? string.Empty : criteria.AId));
            }

            if (criteria.IsIncludeEmpty == "true" || !string.IsNullOrEmpty(criteria.L))
            {
                jsonBuilder.Append(string.Format("\"{0}\":\"{1}\",", "l", criteria.L == null ? string.Empty : criteria.L));
            }

            if (criteria.IsIncludeEmpty == "true" || !string.IsNullOrEmpty(criteria.OS))
            {
                jsonBuilder.Append(string.Format("\"{0}\":\"{1}\",", "os", criteria.OS == null ? string.Empty : criteria.OS));
            }

            if (criteria.IsIncludeEmpty == "true" || !string.IsNullOrEmpty(criteria.CC))
            {
                jsonBuilder.Append(string.Format("\"{0}\":\"{1}\",", "cc", criteria.CC == null ? string.Empty : criteria.CC));
            }

            string criteriaJson = string.Format("{{\"criteria\":{{{0}}}}}", jsonBuilder.ToString().Substring(0, jsonBuilder.Length - 1));

            var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);

            httpWebRequest.ContentType = "application/json; charset=UTF-8";
            httpWebRequest.Method = "POST";

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                streamWriter.Write(Server.UrlEncode(criteriaJson));
                streamWriter.Flush();
                streamWriter.Close();
            }

            StringBuilder result = new StringBuilder(string.Empty);
            try
            {
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    result.Append(streamReader.ReadToEnd());
                }

                if (string.IsNullOrEmpty(result.ToString()))
                {
                    foreach (var item in httpResponse.Headers.AllKeys.Where(p=>p.ToLower().Contains("adfalcon")))
                    {
                        result.AppendLine(string.Format("{0} = {1} <br/>", item, httpResponse.Headers[item].ToString()));
                    }
                }
            }
            catch (Exception x)
            {

            }

            return Content(result.ToString());
        }

    }
}
