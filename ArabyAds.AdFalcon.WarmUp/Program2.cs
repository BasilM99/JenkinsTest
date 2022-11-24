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
    class Program2
    {
        private const string sucessMsg = "{0} '{1}'";
        private const string failedMsg = "{0} '{1}'";
        private const string LoginMsg = "{0} 'Not Logged in User'";

        private static SecurityManager _securityManager;
        private static ISecurityService _securityService;
        private static NoqoushPrincipal _principal;
        static SecurityManager _securityProxy;
        private static HttpCookie _cookie = null;
        private static readonly CookieContainer CookieContainer = new CookieContainer();

        private static void Init()
        {
            _securityService = IoC.Instance.Resolve<ISecurityService>();
            //_appSiteService = Noqoush.Framework.IoC.Instance.Resolve<IAppSiteService>();
            _securityProxy = new SecurityManager(IoC.Instance.Resolve<ISecurityService>());

            _securityManager = new SecurityManager(_securityService);
            //MappingRegister.RegisterMapping();
        }
        private static void Login()
        {
            var isSuccess = false;
            while (!isSuccess)
            {
                AuthenticateResponse response = _securityManager.AuthenticateUser(Config.UserName, Config.Password);
                if (response.Status == AuthenticateStatus.Success)
                {
                    _principal = response.Principal;
                    isSuccess = true;
                    System.Threading.Thread.CurrentPrincipal = _principal;
                    _securityProxy.BuildSecurityContext(_principal.Token);
                    SetCookies(response.Principal.Token);
                    Console.WriteLine("login successfully");

                }
            }
        }
        private static void SetCookies(string token)
        {
            var ticket = new FormsAuthenticationTicket(1, Config.UserName, Framework.Utilities.Environment.GetServerTime(), Framework.Utilities.Environment.GetServerTime().AddMinutes(FormsAuthentication.Timeout.TotalMinutes), true, token);

            string encryptedTicket = FormsAuthentication.Encrypt(ticket);

            _cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket)
                          {
                              Expires = Framework.Utilities.Environment.GetServerTime().AddMinutes(Config.TotalMinutes),
                              Secure = true,
                              Domain = Config.Domain,
                              Name = Config.CookieName
                          };
        }
        private static void CallURL(URL url)
        {
            //  Establish the request
            var request = (HttpWebRequest)WebRequest.Create(url.Value);
            request.CookieContainer = new CookieContainer();
            //request.CookieContainer.Add(_cookie);

            request.Host = "192.168.2.88:8080";
            request.UserAgent = Config.UserAgent;
            request.Headers.Add(HttpRequestHeader.Cookie, string.Format("{0}={1};", Config.CookieName, _cookie.Value));

            // *** Set properties
            request.Timeout = Config.TimeOut;
            request.UserAgent = Config.UserAgent;
            using (HttpWebResponse webResponse = request.GetResponse() as HttpWebResponse)
            {
                Encoding enc = System.Text.Encoding.GetEncoding(1252);
                var loResponseStream = new StreamReader(webResponse.GetResponseStream(), enc);

                string lcHtml = loResponseStream.ReadToEnd();

                if (webResponse.StatusCode == HttpStatusCode.OK)
                {
                    //if (webResponse.ResponseUri.ToString().Contains(Config.LoginURL))
                    //{
                    //    Console.WriteLine(string.Format(LoginMsg, url));
                    //}
                    //else
                    {
                        Console.WriteLine(string.Format(sucessMsg, url.Name, HttpStatusCode.OK));
                    }
                }
                else
                {
                    Console.WriteLine(string.Format(failedMsg, url.Name, webResponse.StatusCode));
                }
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

            // initialize the framework service proxy
            Init();
            // Login to AdFalcon
            Login();

            // read sites
            var sites = XMLReader.GetSiteConfig(SetDataPath() + "\\URLs.xml");
            //loop throw the sites and call its urls
            foreach (var site in sites)
            {
                Console.WriteLine("Calling Login Page for Site '{0}'", site.Name);
                Console.WriteLine("Calling Site '{0}' URLs", site.Name);
                //loop throw the urls and call it
                foreach (var url in site.UrLs)
                {
                    //TODO:Osaleh to replace this line
                    url.Value = string.Format("{0}/{1}/{2}", site.URL, "en", url.Value);
                    CallURL(url);
                }
            }

            // cleanup resources
            Console.WriteLine("Done");
            Console.ReadLine();
        }


    }

}
