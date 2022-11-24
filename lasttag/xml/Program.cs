using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using System.Xml.XPath;

namespace xml
{
    class Program
    {
        static void Main()
        {
            try
            {

                IsValidXml("C:/Users/Administrator/Documents/test.xml", "C:/Users/Administrator/Desktop/VastXsd/vast_2.0.1.xsd","");
                IsValidXml("C:/Users/Administrator/Documents/test.xml", "C:/Users/Administrator/Desktop/VastXsd/vast3_draft.xsd", "");

                IsValidXml("C:/Users/Administrator/Documents/test.xml", "C:/Users/Administrator/Desktop/VastXsd/vast4.xsd", "");

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

           // string xml = new WebClient().DownloadString(url);
         //   XDocument doc = XDocument.Parse(xml);

        }
        public static bool IsValidXml(string xmlFilePath, string xsdFilePath, string namespaceName)
        {
            var xdoc = XDocument.Load(xmlFilePath);
            var schemas = new XmlSchemaSet();
            schemas.Add(namespaceName, xsdFilePath);

            try
            {
                xdoc.Validate(schemas, null);
            }
            catch (XmlSchemaValidationException e)
            {
                return false;
            }

            return true;
        }
        public static bool IsValidXmlWithMethod(string xmlFilePath, string xsdFilePath, string namespaceName)
        {
            var xdoc = XDocument.Load(xmlFilePath);
            var schemas = new XmlSchemaSet();
            schemas.Add(namespaceName, xsdFilePath);

            Boolean result = true;
            xdoc.Validate(schemas, (sender, e) =>
            {
                result = false;
            });

            return result;
        }



  
    }
}

