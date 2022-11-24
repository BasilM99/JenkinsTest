using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
namespace Noqoush.AdFalcon.WarmUp
{
    public class Site
    {
        public string Name { get; set; }
        public string URL { get; set; }
        public string LoginURL { get; set; }
        public string CookieDomain { get; set; }
        public List<URL> UrLs { get; set; }
    }
    public class URL
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }
    public class XMLReader
    {
        public static List<Site> GetSiteConfig(string filePath)
        {
            var result = new List<Site>();
            //load the XML document from the specified file.*/
            // List all Sites 
            XDocument doc = XDocument.Load(filePath);
            var sites = doc.Descendants("Site").ToList();
            foreach (var  xElement in sites)
            {
                //create new site object
                var site = new Site()
                               {
                                   Name = xElement.Attribute("name").Value,
                                   URL = xElement.Attribute("URL").Value,
                                   LoginURL = xElement.Attribute("LoginURL").Value,
                                   CookieDomain = xElement.Attribute("CookieDomain").Value,
                                   UrLs = new List<URL>()
                               };
                var urls=  xElement.Descendants("url");
                foreach (var element in urls)
                {
                    site.UrLs.Add(new URL()
                                      {
                                          Name = element.Attribute("name").Value,
                                          Value = element.Attribute("value").Value,
                                      });
                }
                result.Add(site);
            }
            return result;
        }
    }
}
