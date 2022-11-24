using System;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using Noqoush.Framework;
using Noqoush.Framework.Security;
using System.Web.Security;

namespace Noqoush.AdFalcon.WarmUp
{
    class Program
    {
        private const string sucessMsg = "{0} '{1}'";
        private const string failedMsg = "{0} '{1}'";
        private const string LoginMsg = "{0} 'Not Logged in User'";

        private static readonly CookieContainer CookieContainer = new CookieContainer();
        public static void LoginToSite(Site site)
        {
            var req = (HttpWebRequest)WebRequest.Create(site.LoginURL);
            //-------Setting up headers------------
            req.UserAgent =Config.UserAgent;
            req.Referer = site.LoginURL;
            req.ContentType = "application/x-www-form-urlencoded";
            req.Headers.Add("Accept-Language", "en-us,es-mx;q=0.7,af;q=0.3");
            //Form content type
            req.Method = "POST";
            //data will be send in POST method
            var sw = new StreamWriter(req.GetRequestStream());
            dynamic poststring = "rememberMe=false&Username=" + Config.UserName + "&Password=" + Config.Password;
            try
            {
                req.CookieContainer = CookieContainer;
                sw.Write(poststring);
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.InfoFormat("error" + ex.Message);
            }
            finally
            {
                sw.Close();
            }

            using (var webResponse = req.GetResponse() as HttpWebResponse)
            {
                SaveIncomingCookies(webResponse,site);
            }
        }
        private static void SaveIncomingCookies(HttpWebResponse response, Site site)
        {
            if (response.Headers["Set-Cookie"] != null)
            {
                CookieContainer.SetCookies(new Uri(site.CookieDomain), response.Headers["Set-Cookie"]);
            }
        }

        private static void CallURL(URL url)
        {
            // *** Establish the request
            var request = (HttpWebRequest)WebRequest.Create(url.Value);
            //loHttp.Credentials = CredentialCache.DefaultCredentials;
            //loHttp.PreAuthenticate = true;
            request.AllowAutoRedirect = false;
            request.CookieContainer = CookieContainer;

            // *** Set properties
            request.Timeout = Config.TimeOut;
            request.UserAgent = Config.UserAgent;
            try
            {
                using (HttpWebResponse webResponse = request.GetResponse() as HttpWebResponse)
                {
                    /* Encoding enc = System.Text.Encoding.GetEncoding(1252);
                     var loResponseStream = new StreamReader(webResponse.GetResponseStream(), enc);

                     string lcHtml = loResponseStream.ReadToEnd();
                     */
                    if (webResponse.StatusCode == HttpStatusCode.OK)
                    {
                        if (webResponse.ResponseUri.ToString().Contains(Config.LoginURL))
                        {
                            ApplicationContext.Instance.Logger.InfoFormat(string.Format(LoginMsg, url));
                        }
                        else
                        {
                            ApplicationContext.Instance.Logger.InfoFormat(string.Format(sucessMsg, url.Name,
                                                                                        HttpStatusCode.OK));
                        }
                    }
                    else
                    {
                        ApplicationContext.Instance.Logger.InfoFormat(string.Format(failedMsg, url.Name,
                                                                                    webResponse.StatusCode));
                    }
                }
            }
            catch(Exception ex)
            {
                ApplicationContext.Instance.Logger.Error(ex.ToString);
            }
        }

      
        public static bool AcceptAllCertifications(object sender, System.Security.Cryptography.X509Certificates.X509Certificate certification, System.Security.Cryptography.X509Certificates.X509Chain chain, System.Net.Security.SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }
        static public string SetDataPath()
        {
            string path = Environment.CommandLine;
            while (path.StartsWith("\""))
            {
                path = path.Substring(1, path.Length - 2);
            }
            while (path.EndsWith("\"") || path.EndsWith(" "))
            {
                path = path.Substring(0, path.Length - 2);
            }
            path = Path.GetDirectoryName(path);

            return path;
        }
        static void Main(string[] args)
        {

            // add this to pass the certificate validation
            ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(AcceptAllCertifications);
            // read sites
            var sites = XMLReader.GetSiteConfig(SetDataPath() + "\\URLs.xml");
            //loop throw the sites and call its urls
            foreach (var site in sites)
            {
                ApplicationContext.Instance.Logger.InfoFormat("Calling Login Page for Site '{0}'", site.Name);
                //login to site
                LoginToSite(site);
                ApplicationContext.Instance.Logger.InfoFormat("Calling Site '{0}' URLs", site.Name);
                //loop throw the urls and call it
                foreach (var url in site.UrLs)
                {
                    //TODO:Osaleh to replace this line
                    url.Value = string.Format("{0}/{1}/{2}", site.URL, "en", url.Value);
                    CallURL(url);

                }
            }

            // cleanup resources
            //Console.ReadLine();
        }


    }

}
