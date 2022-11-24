using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Schema;
using System.IO;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Core;
using Noqoush.AdFalcon.Services.Interfaces.Services.Campaign;
using Noqoush.AdFalcon.Services.Interfaces.Services.Core;
using Noqoush.Framework;

namespace Noqoush.AdFalcon.Web.Controllers.Utilities
{
    public class DocumentUtility
    {

        public static bool IsValidXml(string xmlString, string XsdsFolderPath)
        {
            XDocument xmlFile = XDocument.Parse(xmlString);

            bool result = false;
            foreach (string file in System.IO.Directory.EnumerateFiles(XsdsFolderPath, "*.xsd"))
            {
                var schemas = new XmlSchemaSet();
                var lines = File.ReadAllText(file);

                if (lines.Contains(@"targetNamespace="))
                {
                    schemas.Add(@"http://www.iab.com/VAST", file);
                }
                else
                {

                    schemas.Add(@"", file);
                }
                try
                {
                    xmlFile.Validate(schemas, null);
                }
                catch (XmlSchemaValidationException e)
                {
                    result = result || false;
                    continue;
                }
                catch (Exception e)
                {
                    return false;
                }
                result = result || true;
            }

            return result;
        }

        public static XDocument downloadXml(string url)
        {
            XDocument doc = null;
            try
            {
                string xml = new WebClient().DownloadString(url);
                doc = XDocument.Parse(xml);
            }
            catch (Exception e)
            {

                return null;
            }

            return doc;

        }


        public static void SaveVideoThumbnail(string pathToVideoFile, out int docId)
        {

            IDocumentService _documentService = IoC.Instance.Resolve<IDocumentService>();


            IDocumentTypeService _documentTypeService = IoC.Instance.Resolve<IDocumentTypeService>();

            //adCreative.MediaFilesSupported.FirstOrDefault().URL
            MemoryStream ms1 = new MemoryStream();
            var ffMpeg = new NReco.VideoConverter.FFMpegConverter();
            ffMpeg.GetVideoThumbnail(pathToVideoFile, ms1, 1);
            byte[] base64Data = ms1.ToArray();

            var doc = new DocumentDto
            {
                Size = base64Data.Length,
                Extension = ".png",// Path.GetExtension(shellFile.Path),
                Name = Path.GetFileNameWithoutExtension(pathToVideoFile),
                DocumentTypeId = _documentTypeService.GetByCode(".png").ID //_documentTypeService.GetByCode(Request.Files[0].ContentType).ID
            };
            doc.Content = base64Data;// ms.ToArray();


            docId = _documentService.Save(doc);
        }

        public static string CopyFileToFolder(byte[] file, string folder, string name)
        {

            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);

            }
            var FileUrl = folder + "/" + name;
            
            FileStream File = System.IO.File.Create(FileUrl);
            File.Write(file, 0, file.Length);
            File.Close();
            return FileUrl;

        }

        public static void DeleteFile(string FileUrl)
        {

            if ((System.IO.File.Exists(FileUrl)))
            {
                System.IO.File.Delete(FileUrl);
            }

        }



    }
}
