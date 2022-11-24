using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Web;
using System.Web.Mvc;
using Noqoush.AdFalcon.Domain.Common.Model.Campaign;
using Noqoush.AdFalcon.Domain.Common.Model.Core;
using Noqoush.AdFalcon.Domain.Common.Repositories.Campaign;
using Noqoush.AdFalcon.Domain.Common.Repositories.Core;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Campaign.Creative;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Core;
using Noqoush.AdFalcon.Services.Interfaces.Services.Campaign;
using Noqoush.AdFalcon.Services.Interfaces.Services.Campaign.Creative;
using Noqoush.AdFalcon.Services.Interfaces.Services.Core;
using System.Linq;
using Noqoush.AdFalcon.Web.Controllers.Core;
using Noqoush.AdFalcon.Web.Controllers.Model.Core;
using Noqoush.AdFalcon.Web.Controllers.Utilities;
using Microsoft.WindowsAPICodePack.Shell;
using System.Text;
using System.Drawing.Imaging;
using NReco.VideoConverter;

using Noqoush.Framework;
using Noqoush.AdFalcon.Services.Interfaces.Services.Account;
using System.IO.Compression;
using HtmlAgilityPack;
using System.Configuration;
using Noqoush.Framework.Utilities;

namespace Noqoush.AdFalcon.Web.Controllers.Controllers.Core
{
    public class DocumentController : AuthorizedControllerBase
    {
        private readonly IDocumentService _documentService;
        private readonly IDocumentTypeService _documentTypeService;
        private ICreativeUnitService _creativeUnitService;
        private ITileImageService _tileImageService;
        protected IVideoTypeService _videoTypeService;
        protected ICampaignService _campaignService;

        protected IAccountService _accountService;
        public DocumentController(IDocumentService documentService, IDocumentTypeService documentTypeService, ICreativeUnitService creativeUnitService, ITileImageService tileImageService, IVideoTypeService videoTypeService, ICampaignService campaignService, IAccountService AccountService)
        {
            _tileImageService = tileImageService;
            _documentService = documentService;
            _documentTypeService = documentTypeService;
            _creativeUnitService = creativeUnitService;
            _campaignService = campaignService;

            _accountService = AccountService;
        }
        private Image resizeImage(Image imgToResize, Size size)
        {
            int sourceWidth = imgToResize.Width;
            int sourceHeight = imgToResize.Height;
            if ((sourceHeight == size.Height) && (sourceWidth == size.Width))
            {
                return imgToResize;
            }

            Bitmap b = new Bitmap(size.Width, size.Height);
            Graphics g = Graphics.FromImage((Image)b);
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.DrawImage(imgToResize, 0, 0, size.Width, size.Height);
            g.Dispose();
            return (Image)b;
        }
        public byte[] ImageToByteArray(System.Drawing.Image imageIn, System.Drawing.Image sourceImageIn = null)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                if (sourceImageIn != null)
                {
                    imageIn.Save(ms, sourceImageIn.RawFormat);
                }
                else
                {
                    imageIn.Save(ms, imageIn.RawFormat);
                }
                return ms.ToArray();
            }
        }
        public byte[] ReadFully(Stream input, int length)
        {
            var buffer = new byte[length];
            using (var ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }
        public Image ByteArrayToImage(byte[] byteArrayIn)
        {
            using (var ms = new MemoryStream(byteArrayIn))
            {
                var returnImage = Image.FromStream(ms);
                return returnImage;
            }
        }
        //private static Id64Generator _id64Generator = new Id64Generator(int.Parse(System.Configuration.ConfigurationManager.AppSettings["HostId"]));
        public ActionResult SaveTile(HttpPostedFileBase attachment, int parentId)
        {
            var documentId = 0;
            var status = "OK";
            if (Request.Files.Count == 0)
            {
                status = ResourcesUtilities.GetResource("NoFile", "Upload");
            }
            else
            {
                var fileToUpload = Request.Files[0];

                if (fileToUpload.ContentLength != 0)
                {

                    TileImageSizeDto sizeDTO = _tileImageService.GetSizeByParentId(parentId);
                    string extension = Path.GetExtension(fileToUpload.FileName).ToLower();

                    FormatDto format = sizeDTO.Formats.Where(p => p.Format == extension).SingleOrDefault();

                    if (format == null)
                    {
                        status = ResourcesUtilities.GetResource("FileType", "Upload");
                    }
                    else
                    {
                        var sourceImg = Image.FromStream(fileToUpload.InputStream);

                        int tileWidth = sizeDTO.Width;
                        int tileHeight = sizeDTO.Height;

                        var size = new Size() { Width = tileWidth, Height = tileHeight };

                        //check file width/Height
                        if (sourceImg.Width == size.Width && sourceImg.Height == size.Height)
                        {

                            var buffer = ImageToByteArray(sourceImg);

                            if (buffer.Length > format.MaxSize)
                            {
                                status = string.Format(ResourcesUtilities.GetResource("FileSize1", "Upload"), format.MaxSize / 1024);
                            }
                            else
                            {
                                var doc = new DocumentDto
                                {
                                    Size = fileToUpload.ContentLength,
                                    Extension = Path.GetExtension(fileToUpload.FileName),
                                    Name = Path.GetFileNameWithoutExtension(fileToUpload.FileName),
                                    Content = buffer,
                                    DocumentTypeId = 1
                                };
                                documentId = _documentService.Save(doc);
                            }
                        }
                        else
                        {
                            status = ResourcesUtilities.GetResource("ImageDimensions", "Upload");
                        }
                    }
                }
                else
                {
                    status = ResourcesUtilities.GetResource("FileSize", "Upload");
                }
            }
            // Return an empty string to signify success
            return Json(new { status = status, DocumentId = documentId }, "text/plain");
        }

        public ActionResult Save(HttpPostedFileBase attachment, int? adTypeId, string group, int? parentId, int typeId, int AdSubTypeId = 0)
        {
            var documentId = 0;
            var creativeUnitId = 0;
            int width, height;
            width = height = 0;
            var status = "OK";
            if (Request.Files.Count == 0)
            {
                status = ResourcesUtilities.GetResource("NoFile", "Upload");
            }
            else
            {
                if (parentId == 8)
                {
                    typeId = 0;
                }
                var typeIdvar = typeId;
                if (AdSubTypeId == (int)Noqoush.AdFalcon.Domain.Common.Model.Campaign.AdSubTypes.HTML5RichMedia || AdSubTypeId == (int)Noqoush.AdFalcon.Domain.Common.Model.Campaign.AdSubTypes.HTML5Interstitial)
                {
                    typeIdvar = 0;
                }
                var creativeUnits = _creativeUnitService.GetByCriteria(parentId, typeIdvar, group, adTypeId);
                if (creativeUnits != null && creativeUnits.Count != 0)
                {
                    var hpf = Request.Files[0];
                    if (hpf.ContentLength != 0)
                    {
                        using (var sourceImg = Image.FromStream(hpf.InputStream))
                        {
                            var creativeUnit = creativeUnits.Where(p => (p.Width == sourceImg.Width && p.Height == sourceImg.Height) ||
                                                                        (p.HD_Width == sourceImg.Width && p.HD_Height == sourceImg.Height)).SingleOrDefault();

                            if (creativeUnit != null)
                            {
                                creativeUnitId = creativeUnit.ID;
                                var format = GetCreativeUnitFormat(creativeUnit.Formats, Path.GetExtension(hpf.FileName));
                                if (format == null)
                                {
                                    status = ResourcesUtilities.GetResource("FileType", "Upload");
                                }
                                else
                                {
                                    //var img = resizeImage(sourceImg, size);
                                    //read file content and get image as bytes
                                    var buffer = ImageToByteArray(sourceImg);
                                    if (buffer.Length > format.MaxSize)
                                    {
                                        status = string.Format(ResourcesUtilities.GetResource("FileSize1", "Upload"), format.MaxSize / 1024);
                                    }
                                    else
                                    {
                                        //check File Size
                                        if ((format != null) && (buffer.Length <= format.MaxSize))
                                        {
                                            width = sourceImg.Width;
                                            height = sourceImg.Height;
                                            var doc = new DocumentDto
                                            {
                                                Size = hpf.ContentLength,
                                                Extension = Path.GetExtension(hpf.FileName),
                                                Name = Path.GetFileNameWithoutExtension(hpf.FileName),
                                                Content = buffer,
                                                DocumentTypeId = _documentTypeService.GetByCode(format.Format).ID
                                            };
                                            documentId = _documentService.Save(doc);
                                            status = "OK";
                                        }
                                    }
                                }
                            }
                            else
                            {
                                status = ResourcesUtilities.GetResource("ImageDimensions", "Upload");
                            }
                        }
                    }
                    else
                    {
                        status = ResourcesUtilities.GetResource("FileSize", "Upload");
                    }
                }
                else
                {
                    status = ResourcesUtilities.GetResource("NotValid", "Upload");
                }
            }

            // Return an empty string to signify success
            return Json(new { status = status, DocumentId = documentId, CreativeUnitId = creativeUnitId, Width = width, Height = height }, "text/plain");
        }

        public ActionResult SaveForBulk(HttpPostedFileBase attachment, int typeId, int? adTypeId)
        {
            var documentId = 0;
            var creativeUnitId = 0;
            int width, height;
            width = height = 0;
            var status = "OK";
            if (Request.Files.Count == 0)
            {
                status = ResourcesUtilities.GetResource("NoFile", "Upload");
            }
            else if (Path.GetExtension(Request.Files[0].FileName) == ".txt" || Path.GetExtension(Request.Files[0].FileName) == ".TXT")
            {
                var PathName = Path.GetFileName(Request.Files[0].FileName);
                PathName = PathName.Replace("txt", string.Empty);
                PathName = PathName.Replace("TXT", string.Empty);

                PathName = PathName.Replace(".txt", string.Empty);
                PathName = PathName.Replace(".TXT", string.Empty);

                var widthstr = PathName.Substring(0, PathName.IndexOf("x"));

                var heightstr = PathName.Substring(PathName.IndexOf("x") + 1);
                int.TryParse(widthstr, out width);
                int.TryParse(heightstr, out height);

                if (width == 0 || height == 0)
                {
                    status = ResourcesUtilities.GetResource("ImageDimensions", "Upload");
                    return Json(new { status = status, DocumentId = documentId, CreativeUnitId = creativeUnitId, Width = width, Height = height }, "text/plain");
                }

                var creativeUnits = _creativeUnitService.GetByCriteriaWidthHeight(null, typeId, "", adTypeId, width, height);

                var creativeUnit = creativeUnits.Where(p => (p.Width == width && p.Height == height) ||
                                                                 (p.HD_Width == width && p.HD_Height == height)).SingleOrDefault();

                if (creativeUnit != null)
                {
                    string fileContents;
                    var hpf = Request.Files[0];
                    using (StreamReader reader = new StreamReader(hpf.InputStream))
                    {
                        status = "OK";
                        fileContents = reader.ReadToEnd();


                    }

                    return Json(new { status = status, fileContents = fileContents, CreativeUnitId = creativeUnitId, Width = width, Height = height }, "text/plain");
                }

            }
            else
            {


                var hpf = Request.Files[0];
                if (hpf.ContentLength != 0)
                {
                    using (var sourceImg = Image.FromStream(hpf.InputStream))
                    {
                        var creativeUnits = _creativeUnitService.GetByCriteriaWidthHeight(null, typeId, "", adTypeId, sourceImg.Width, sourceImg.Height);

                        var creativeUnit = creativeUnits.Where(p => (p.Width == sourceImg.Width && p.Height == sourceImg.Height) ||
                                                                    (p.HD_Width == sourceImg.Width && p.HD_Height == sourceImg.Height)).SingleOrDefault();

                        if (creativeUnit != null)
                        {
                            creativeUnitId = creativeUnit.ID;
                            var format = GetCreativeUnitFormat(creativeUnit.Formats, Path.GetExtension(hpf.FileName));
                            if (format == null)
                            {
                                status = ResourcesUtilities.GetResource("FileType", "Upload");
                            }
                            else
                            {
                                //var img = resizeImage(sourceImg, size);
                                //read file content and get image as bytes
                                var buffer = ImageToByteArray(sourceImg);
                                if (buffer.Length > format.MaxSize)
                                {
                                    status = string.Format(ResourcesUtilities.GetResource("FileSize1", "Upload"), format.MaxSize / 1024);
                                }
                                else
                                {
                                    //check File Size
                                    if ((format != null) && (buffer.Length <= format.MaxSize))
                                    {
                                        width = sourceImg.Width;
                                        height = sourceImg.Height;
                                        var doc = new DocumentDto
                                        {
                                            Size = hpf.ContentLength,
                                            Extension = Path.GetExtension(hpf.FileName),
                                            Name = Path.GetFileNameWithoutExtension(hpf.FileName),
                                            Content = buffer,
                                            DocumentTypeId = _documentTypeService.GetByCode(format.Format).ID
                                        };
                                        documentId = _documentService.Save(doc);
                                        status = "OK";
                                    }
                                }
                            }
                        }
                        else
                        {
                            status = ResourcesUtilities.GetResource("ImageDimensions", "Upload");
                        }
                    }
                }
                else
                {
                    status = ResourcesUtilities.GetResource("FileSize", "Upload");
                }

            }

            // Return an empty string to signify success
            return Json(new { status = status, DocumentId = documentId, CreativeUnitId = creativeUnitId, Width = width, Height = height }, "text/plain");
        }
        public ActionResult SaveForZip(HttpPostedFileBase attachment, int typeId, int? adTypeId, int? AdSubTypeId)
        {
            var documentId = 0;
            var creativeUnitId = 0;
            int width, height;
            width = height = 0;
            var status = "OK";
            string FileName = string.Empty;
            Stream data = null;
            IList<ClickTagTrackerDto> clickTags = new List<ClickTagTrackerDto>();
            if (Request.Files.Count == 0)
            {
                status = ResourcesUtilities.GetResource("NoFile", "Upload");
            }
            else if (Path.GetExtension(Request.Files[0].FileName) != ".zip" && Path.GetExtension(Request.Files[0].FileName) == ".ZIP")
            {
                status = ResourcesUtilities.GetResource("ZipFile", "Upload");


            }
            else
            {



                var hpf = Request.Files[0];
                if (hpf.ContentLength != 0)
                {
                    data = hpf.InputStream; // The original data
                    Stream unzippedEntryStream = null; // Unzipped data from a file in the archive
                    bool indexFound = false;
                    bool getParamaerrFunc = false;
                    bool ClickTagFunc = false;
                    int b = (int)hpf.ContentLength;
                    decimal kb = (b / 1024);

                    if (kb > Config.SizeHTML5)
                    {
                        status = ResourcesUtilities.GetResource("FileSize", "Upload");
                        return Json(new { status = status, DocumentId = documentId, CreativeUnitId = creativeUnitId, Width = width, Height = height }, "text/plain");
                    }

                    byte[] buffer;
                    using (MemoryStream ms = new MemoryStream())
                    {
                        data.CopyTo(ms);




                        buffer = ms.ToArray();
                    }

                    ZipArchive archive = new ZipArchive(data);
                    foreach (ZipArchiveEntry entry in archive.Entries)
                    {
                        if (entry.FullName.ToLower().Equals("index.html", StringComparison.OrdinalIgnoreCase) || entry.FullName.ToLower().Equals("index.htm", StringComparison.OrdinalIgnoreCase))
                        {
                            if (indexFound == false)
                            {
                                unzippedEntryStream = entry.Open(); // .Open will return a stream
                                indexFound = true;                         // Process entry data here
                            }
                            //else
                            //{
                            //    status = ResourcesUtilities.GetResource("ZipFileIndex", "Upload");
                            //    return Json(new { status = status, DocumentId = documentId, CreativeUnitId = creativeUnitId, Width = width, Height = height }, "text/plain");
                            //}
                        }
                    }
                    if (indexFound == false)
                    {
                        status = ResourcesUtilities.GetResource("ZipFileIndex", "Upload");
                        return Json(new { status = status, DocumentId = documentId, CreativeUnitId = creativeUnitId, Width = width, Height = height }, "text/plain");
                    }


                    // using ()
                    {
                        // Read the stream to a string, and write the string to the console.
                        StreamReader sr = new StreamReader(unzippedEntryStream);

                        String line = sr.ReadToEnd();


                        var doc = new HtmlDocument();
                        doc.LoadHtml(line);
                        if (doc.ParseErrors.Count() > 0)
                        {
                            //Invalid HTML

                            status = ResourcesUtilities.GetResource("InvalidHTML", "Global");
                            return Json(new { status = status, DocumentId = documentId, CreativeUnitId = creativeUnitId, Width = width, Height = height }, "text/plain");

                        }
                        foreach (HtmlNodeNavigator link in doc.CreateNavigator().Select("//meta[@name]"))
                        {
                            var nameString = link.CurrentNode.GetAttributeValue("name", "");
                            var contentString = link.CurrentNode.GetAttributeValue("content", "");


                            if (nameString.ToLower() == "ad.size")
                            {
                                if (contentString != string.Empty && contentString.ToLower().Contains("width"))
                                {
                                    var listdim = contentString.Split(',');
                                    var widthtsrin = listdim[0].Replace("width=", string.Empty);
                                    var heightsrin = listdim[1].Replace("height=", string.Empty);

                                    int.TryParse(widthtsrin, out width);
                                    int.TryParse(heightsrin, out height);

                                }
                            }
                        }
                        bool foundDuplicated = false;
                        foreach (HtmlNodeNavigator script in doc.CreateNavigator().Select("//script"))
                        {
                            var scriptText = script.CurrentNode.InnerHtml;
                            //var startIndex = scriptText.ToLower().IndexOf("getparameterbyname", System.StringComparison.Ordinal);
                            int startIndex = 0;
                            int endIndex = 0;
                            int newendIndex = 0;
                            // foundDuplicated = false;
                            //if (startIndex>0)
                            //{
                            //    getParamaerrFunc = true;

                            //}

                            var startclickTagIndex = scriptText.ToLower().IndexOf("clicktag", System.StringComparison.Ordinal);
                            if (startclickTagIndex > 0)
                            {
                                ClickTagFunc = true;

                            }
                            scriptText = scriptText.ToLower();
                            bool equalclickTag = true;
                            do
                            {

                                startIndex = scriptText.IndexOf("clicktag");
                                endIndex = getFirstAppearance(scriptText, startIndex + "clicktag".Length, startIndex, new char[] { '\'', '\"' }, out char foundSeek);
                                newendIndex = 0;
                                if (endIndex > 0 && startIndex > 0)
                                {
                                    string orString = scriptText.Substring(startIndex, endIndex - startIndex);
                                    if (!orString.Contains("="))
                                    {
                                        equalclickTag = false;
                                    }
                                    else
                                    {
                                        equalclickTag = true;
                                    }
                                    orString = orString.Replace("=", "").Replace("'", "").Replace("\"", "").Trim();
                                    endIndex = endIndex + 1;
                                    newendIndex = getFirstAppearance(scriptText, endIndex + orString.Length, endIndex, new char[] { ';', '\'', '\"' }, out foundSeek);

                                    string urlOString = scriptText.Substring(endIndex, newendIndex - endIndex);
                                    urlOString = urlOString.Replace("=", "").Replace("'", "").Replace("\"", "").Trim();

                                    startIndex = newendIndex;
                                    if (clickTags.Where(M => M.VariableName == orString).SingleOrDefault() == null && equalclickTag)
                                    {
                                        clickTags.Add(new ClickTagTrackerDto { TrackingUrl = urlOString, VariableName = orString });
                                    }
                                    else
                                    {
                                        if (equalclickTag)
                                        {
                                            foundDuplicated = true;
                                            break;
                                        }
                                    }


                                }
                                if (startIndex > 0)
                                    scriptText = scriptText.Substring(startIndex);

                                startIndex = scriptText.IndexOf("clicktag");
                            }
                            while (startIndex > 0);


                        }

                        if (!(ClickTagFunc))
                        {
                            //Invalid HTML

                            status = ResourcesUtilities.GetResource("ZipFileIndexClick", "Upload");
                            return Json(new { status = status, DocumentId = documentId, CreativeUnitId = creativeUnitId, Width = width, Height = height }, "text/plain");

                        }
                        if ((foundDuplicated))
                        {
                            //Invalid HTML

                            status = ResourcesUtilities.GetResource("ZipFileIndexDupClick", "Upload");
                            return Json(new { status = status, DocumentId = documentId, CreativeUnitId = creativeUnitId, Width = width, Height = height }, "text/plain");

                        }



                    }

                    //using (MemoryStream ms = new MemoryStream())
                    //{
                    //    int read;
                    //    while ((read = data.Read(buffer, 0, buffer.Length)) > 0)
                    //    {
                    //        ms.Write(buffer, 0, read);
                    //    }
                    //    buffer = ms.ToArray();
                    //}

                    FileName = Path.GetFileNameWithoutExtension(hpf.FileName);
                    var creativeUnits = _creativeUnitService.GetByCriteriaWidthHeight(null, 0, "", 4, width, height);

                    var creativeUnit = creativeUnits.Where(p => (p.Width == width && p.Height == height) ||
                                                                (p.HD_Width == width && p.HD_Height == height)).SingleOrDefault();

                    if (creativeUnit != null)
                    {
                        creativeUnitId = creativeUnit.ID;


                        {
                            //var img = resizeImage(sourceImg, size);
                            //read file content and get image as bytes


                            {
                                //check File Size
                                //if ((format != null) && (buffer.Length <= format.MaxSize))
                                // {

                                var doc = new DocumentDto
                                {
                                    Size = hpf.ContentLength,
                                    Extension = Path.GetExtension(hpf.FileName),
                                    Name = Path.GetFileNameWithoutExtension(hpf.FileName),
                                    Content = buffer,
                                    DocumentTypeId = _documentTypeService.GetByCode(".zip").ID
                                };
                                documentId = _documentService.Save(doc);
                                status = "OK";
                                //}
                            }
                        }
                    }
                    else
                    {
                        //  status = ResourcesUtilities.GetResource("ImageDimensions", "Upload");


                        var doc = new DocumentDto
                        {
                            Size = hpf.ContentLength,
                            Extension = Path.GetExtension(hpf.FileName),
                            Name = Path.GetFileNameWithoutExtension(hpf.FileName),
                            Content = buffer,
                            DocumentTypeId = _documentTypeService.GetByCode(".zip").ID
                        };
                        documentId = _documentService.Save(doc);
                        status = "OK";
                    }

                }
                else
                {
                    status = ResourcesUtilities.GetResource("FileSize", "Upload");
                }

            }
            if (!string.IsNullOrEmpty(FileName))
            {
                FileName = " " + FileName;
            }
            // Return an empty string to signify success
            return Json(new { status = status, DocumentId = documentId, CreativeUnitId = creativeUnitId, Width = width, Height = height, FileName = FileName, clickTags = clickTags }, "text/plain");
        }


        public ActionResult SaveForCSV(HttpPostedFileBase attachment, int AudienceListId, int DeviceTypeId)
        {
            var documentId = 0;
            
     
            var status = "OK";
            string FileName = string.Empty;
            Stream data = null;
           
            if (Request.Files.Count == 0)
            {
                status = ResourcesUtilities.GetResource("NoFile", "Upload");
            }
            else if (!((Path.GetExtension(Request.Files[0].FileName) == ".csv" || Path.GetExtension(Request.Files[0].FileName) == ".CSV" ) || (Path.GetExtension(Request.Files[0].FileName) == ".txt" || Path.GetExtension(Request.Files[0].FileName) == ".TXT")))
            {
                status = ResourcesUtilities.GetResource("CSVFile", "Upload");


            }
            else
            {



                var hpf = Request.Files[0];
                if (hpf.ContentLength != 0)
                {
                    data = hpf.InputStream; // The original data
                 
                    int b = (int)hpf.ContentLength;
                    decimal kb = (b / 1024);

                    if (kb > Config.SizeCSV)
                    {
                        status = ResourcesUtilities.GetResource("FileSize", "Upload");
                        return Json(new { status = status, DocumentId = documentId}, "text/plain");
                    }

                    //byte[] buffer;
                    //using (MemoryStream ms = new MemoryStream())
                    //{
                    //    data.CopyTo(ms);




                    //    buffer = ms.ToArray();
                    //}

                    string fileExtension = Path.GetExtension(hpf.FileName);
                    Guid id = Guid.NewGuid();
                    FileName = "Devices_Id_" + AudienceListId + "_" + DeviceTypeId + "_"+ id.ToString() + fileExtension;
                    
                    var uploadFilesDir = System.Configuration.ConfigurationManager.AppSettings["DeviceIdFilesPath"];

                    if (!Directory.Exists(uploadFilesDir))
                    {
                        Directory.CreateDirectory(uploadFilesDir);

                    }
                    //throw new Exception();
                    
                    var fileSavePath = Path.Combine(uploadFilesDir, FileName);

                    hpf.SaveAs(fileSavePath);
                }
                else
                {
                    status = ResourcesUtilities.GetResource("FileSize", "Upload");
                }

            }
            if (!string.IsNullOrEmpty(FileName))
            {
                FileName = " " + FileName;
            }
            // Return an empty string to signify success
            return Json(new { status = status, DocumentId = documentId,  FileName = FileName }, "text/plain");
        }
        public int getFirstAppearance(string content,int FoundStartIndex, int startIndex, char[] seek, out char foundSeek)
        {
            if (!(FoundStartIndex >= 0))
            {
                foundSeek = ' ';
                return -1;
            }
            if (!(startIndex>0))
            {
                foundSeek = ' ';
                return -1;
            }
            string lowerrContent = content;
            for (int i = startIndex; i < lowerrContent.Length; i++)
            {
                for (int z = 0; z < seek.Count(); z++)
                {
                    if (lowerrContent[i] == seek[z])
                    {
                        foundSeek = seek[z];
                        return i;
                    }

                }

            }
            foundSeek = ' ';
            return -1;
        }
        public FileResult DownloadZipFile(int documentId)
        {
          
                var documentDto = _documentService.Get(documentId);
                byte[] fileBytes = documentDto.Content;
                string fileName = documentDto.Name/*+documentDto.Extension*/;
                return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
            
          
        }
        [RequireHttps]
        public ActionResult SaveHttps(HttpPostedFileBase attachment)
        {
            var documentId = 0;
            var creativeUnitId = 0;
            int width, height;
            width = height = 0;
            var status = "OK";
            if (Request.Files.Count == 0)
            {
                status = ResourcesUtilities.GetResource("NoFile", "Upload");
            }
            else
            {
                var hpf = Request.Files[0];
                if (hpf.ContentLength != 0)
                {
                    using (var sourceImg = Image.FromStream(hpf.InputStream))
                    {

                        if (Path.GetExtension(hpf.FileName) != ".png")
                        {
                            status = ResourcesUtilities.GetResource("FileType", "Upload");
                        }
                        else
                        {
                            //var img = resizeImage(sourceImg, size);
                            //read file content and get image as bytes
                            var buffer = ImageToByteArray(sourceImg);


                            //check File Size

                            width = sourceImg.Width;
                            height = sourceImg.Height;
                            var doc = new DocumentDto
                            {
                                Size = hpf.ContentLength,
                                Extension = Path.GetExtension(hpf.FileName),
                                Name = Path.GetFileNameWithoutExtension(hpf.FileName),
                                Content = buffer,
                                DocumentTypeId = _documentTypeService.GetByCode(".png").ID
                            };
                            documentId = _documentService.Save(doc);
                            status = "OK";

                        }


                    }
                }
                else
                {
                    status = ResourcesUtilities.GetResource("FileSize", "Upload");
                }

            }

            // Return an empty string to signify success
            return Json(new { status = status, DocumentId = documentId, CreativeUnitId = creativeUnitId, Width = width, Height = height, onchange = "onImageChanged" }, "text/plain");
        }

        public ActionResult SaveStreamVideo(HttpPostedFileBase attachment, int? adTypeId, string group, int? parentId, int typeId)
        {

            var documentId = 0;
            int thumbnailDocId = -1;
            var creativeUnitId = 0;
            var status = "OK";
            var imgThumbnail = string.Empty;
            var videoInformation = new VideoInformationModel();



            try
            {
                if (Request.Files.Count == 0)
                {
                    status = ResourcesUtilities.GetResource("NoFile", "Upload");
                }
                else
                {
                    var creativeUnits = _creativeUnitService.GetBy((DeviceTypeEnum)typeId, AdTypeIds.InStreamVideo, AdSubTypes.VideoLinear, group).ToList();


                    if (creativeUnits != null && creativeUnits.Count != 0)
                    {


                        var hpf = Request.Files[0];
                        if (hpf.ContentLength != 0)
                        {
                            var videoType = _campaignService.GetMIMEType("video/" + Path.GetExtension(hpf.FileName).Replace(".", "").Trim());
                            if (videoType >0)
                            {
                                var uploadFilesDir = System.Web.HttpContext.Current.Server.MapPath("~/UploadedVideos");

                                if (!Directory.Exists(uploadFilesDir))
                                {
                                    Directory.CreateDirectory(uploadFilesDir);

                                }
                                //throw new Exception();

                                var fileSavePath = Path.Combine(uploadFilesDir, hpf.FileName);

                                hpf.SaveAs(fileSavePath);

                                videoInformation = GetVideoInformation(fileSavePath);
                                if (videoInformation.Duration <= (Config.InstreamVideoDuraionLimit))
                                {
                                    int frameWidth = videoInformation.Width;
                                    int frameHeight = videoInformation.Height;
                                    videoInformation.FormatName = "video/" + Path.GetExtension(hpf.FileName).Replace(".", "").Trim();
                                    var OrginalcreativeUnit = _creativeUnitService.GetById(21);
                                    var creativeUnit = creativeUnits.Where(p => (p.Width == frameWidth && p.Height == frameHeight) ||
                                                                                    (p.HD_Width == frameWidth && p.HD_Height == frameHeight)).SingleOrDefault();
                                    if (creativeUnit != null)
                                    {




                                        creativeUnitId = creativeUnit.ID;
                                        var format = GetCreativeUnitFormat(OrginalcreativeUnit.Formats, Path.GetExtension(hpf.FileName));
                                        if (format == null)
                                        {
                                            status = ResourcesUtilities.GetResource("FileType", "Upload");
                                        }
                                        else
                                        {
                                            var buffer = new byte[Request.Files[0].ContentLength];
                                            hpf.InputStream.Read(buffer, 0, Request.Files[0].ContentLength);


                                            if (buffer.Length > format.MaxSize)
                                            {
                                                status = string.Format(ResourcesUtilities.GetResource("FileSize1", "Upload"), format.MaxSize / 1024);
                                            }
                                            else
                                            {


                                                var doc = new DocumentDto
                                                {
                                                    Size = hpf.ContentLength,
                                                    Extension = Path.GetExtension(hpf.FileName),
                                                    Name = Path.GetFileNameWithoutExtension(hpf.FileName),
                                                    //  Content = 
                                                    DocumentTypeId = _documentTypeService.GetByCode(Request.Files[0].ContentType).ID
                                                };
                                                doc.Content = buffer;// new byte[Request.Files[0].ContentLength];
                                                //  hpf.InputStream.ReadAsync(doc.Content, 0, Request.Files[0].ContentLength);
                                                doc.IsWebHDFS = true;
                                                var subFolder = Framework.Utilities.Environment.GetServerTime().ToString("yyyyMMdd");
                                            
                                               //doc.Name = doc.Name + doc.Extension;
                                                documentId = _documentService.Save(doc);
                                                status = "OK";

                                                SaveVideoThumbnail(fileSavePath, out thumbnailDocId);


                                            }
                                        }
                                    }
                                    else
                                    {
                                        //
                                        status = ResourcesUtilities.GetResource("InValidResolutions", "Upload");

                                    }
                                }
                                else
                                {
                                    status = string.Format(ResourcesUtilities.GetResource("VideoDurationLimitaionMsgFormat", "Global"), Config.InstreamVideoDuraionLimit);


                                    // status = "Video Duration Should be less 30sec";
                                }
                                System.IO.File.Delete(fileSavePath);
                            }

                            else
                            {
                                status = ResourcesUtilities.GetResource("FileSize", "Upload");
                            }


                        }
                        else
                        {
                            status = ResourcesUtilities.GetResource("FileType", "Upload");
                        }
                    }

                    else
                    {
                        status = ResourcesUtilities.GetResource("NotValid", "Upload");
                    }
                }
            }
            catch (Exception ex)
            {


                ApplicationContext.Instance.Logger.Error(ex.Message, ex);

                return Json(new
                {
                    img = imgThumbnail,
                    status = ResourcesUtilities.GetResource("Exception", "Global"),
                    VideoDocumentId = documentId,
                    ThumbnailDocId = thumbnailDocId,
                    CreativeUnitId = creativeUnitId,
                    VideoInformation = videoInformation,

                    videoName = Request.Files[0].FileName
                }, "text/plain");


            }




            return Json(new
            {
                img = imgThumbnail,
                status = status,
                VideoDocumentId = documentId,
                ThumbnailDocId = thumbnailDocId,
                CreativeUnitId = creativeUnitId,
                VideoInformation = videoInformation,

                videoName = Request.Files[0].FileName
            }, "text/plain");
        }

        private void SaveVideoThumbnail(string pathToVideoFile, out int docId)
        {

            DocumentUtility.SaveVideoThumbnail(pathToVideoFile, out docId);

        }

        public FileResult DownloadTaxDocument(int documentId)
        {
          var checkResult =  _accountService.CheckIfDocumentbelongToAccount(documentId);

            if (checkResult)
            {
                var documentDto = _documentService.Get(documentId);
                byte[] fileBytes = documentDto.Content;
                string fileName = documentDto.Name/*+documentDto.Extension*/;
                return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
            }
            return null;
        }
        private VideoInformationModel GetVideoInformation(string pathToVideoFile)
        {
            AddMessages("GetVideoInformation " + pathToVideoFile, MessagesType.Information);

            MediaInfo uploadedFile = new MediaInfo();

            var result = uploadedFile.Open(pathToVideoFile);
            var streamKind = StreamKind.Video;
            var infoKind = InfoKind.Text;

            var Videoduration = (int)(Math.Floor(Convert.ToDouble(uploadedFile.Get(streamKind, 0, "Duration", infoKind)) / 1000));
            var Audioduration = (int)(Math.Floor(Convert.ToDouble(uploadedFile.Get(StreamKind.Audio, 0, "Duration", infoKind)) / 1000));
            var width = Convert.ToInt16(uploadedFile.Get(streamKind, 0, "Width", infoKind));
            var height = Convert.ToInt16(uploadedFile.Get(streamKind, 0, "Height", infoKind));
            var OverallBitRate = (int)(Math.Round(Convert.ToDouble(uploadedFile.Get(StreamKind.General, 0, "OverallBitRate", infoKind)) / 1000));



            return new VideoInformationModel
            {
                BitRate = OverallBitRate,
                Duration = Videoduration > Audioduration ? Videoduration : Audioduration,
                Width = width,
                Height = height,

            };


        }



        [RequireHttps(Order = 1)]
        public ActionResult SaveAttachment(HttpPostedFileBase attachment, int typeId)
        {
            var documentId = 0;
            var documentName = string.Empty;
            var status = "OK";
            if (Request.Files.Count == 0)
            {
                status = ResourcesUtilities.GetResource("NoFile", "Upload");
            }
            else
            {
                //get attachment unit
                var hpf = Request.Files[0];
                if (hpf.ContentLength != 0)
                {
                    {
                        //read file content and get file as bytes
                        var buffer = ReadFully(hpf.InputStream, hpf.ContentLength);
                        var doc = new DocumentDto
                        {
                            Size = hpf.ContentLength,
                            Extension = Path.GetExtension(hpf.FileName),
                            Name = Path.GetFileNameWithoutExtension(hpf.FileName),
                            Content = buffer,
                            DocumentTypeId = typeId
                        };
                        documentId = _documentService.Save(doc);
                        documentName = doc.Name;
                        status = "OK";
                    }
                }
            }
            return Json(new { status = status, DocumentId = documentId, DocumentName = documentName }, "text/plain");
        }

        [RequireHttps(Order = 1)]
        public ActionResult RemoveHttps()
        {
            // Return an empty string to signify success
            return Json(new { status = "OK", onchange = "onImageChanged" }, "text/plain");
        }


        public ActionResult Copy(int documentId, int parentId, int typeId, int adTypeId)
        {
            AdTypeIds adType = (AdTypeIds)adTypeId;
            var returnFiles = new List<UploadViewModel>();
            var doc = _documentService.Get(documentId);

            if (doc != null)
            {
                using (var sourceImg = ByteArrayToImage(doc.Content))
                {
                    var creativeUnits = _creativeUnitService.GetBy((DeviceTypeEnum)typeId, adType, null, null);
                    if (creativeUnits != null)
                    {
                        var creativeUnitsToCopy = creativeUnits.Where(item => item.ID != parentId);
                        foreach (var creativeUnitDto in creativeUnitsToCopy)
                        {
                            //create document for this Creative Unit
                            var size = new Size() { Width = creativeUnitDto.Width, Height = creativeUnitDto.Height };
                            using (var copyImg = resizeImage(sourceImg, size))
                            {
                                var buffer = ImageToByteArray(copyImg, sourceImg);
                                var copyDoc = new DocumentDto
                                {
                                    Size = buffer.Length,
                                    Extension = doc.Extension,
                                    Name = string.Format("{0}_{1}_{2}", doc.CurrentNameUp, creativeUnitDto.Width, creativeUnitDto.Height),
                                    Content = buffer,
                                    DocumentTypeId = 1
                                };
                                documentId = _documentService.Save(copyDoc);
                            }
                            returnFiles.Add(new UploadViewModel()
                            {
                                ParentId = creativeUnitDto.ID.ToString(),
                                TypeId = typeId.ToString(),
                                DocId = documentId
                            });
                        }
                    }
                    var creativeUnitAnys = _creativeUnitService.GetBy(DeviceTypeEnum.Any, adType, null, null);
                    if (creativeUnitAnys != null)
                    {
                        var creativeUnitsToCopya = creativeUnitAnys.Where(item => item.ID != parentId).Where(M=>M.ID==8);
                        foreach (var creativeUnitDto in creativeUnitsToCopya)
                        {
                            //create document for this Creative Unit
                            var size = new Size() { Width = creativeUnitDto.Width, Height = creativeUnitDto.Height };
                            using (var copyImg = resizeImage(sourceImg, size))
                            {
                                var buffer = ImageToByteArray(copyImg, sourceImg);
                                var copyDoc = new DocumentDto
                                {
                                    Size = buffer.Length,
                                    Extension = doc.Extension,
                                    Name = string.Format("{0}_{1}_{2}", doc.CurrentNameUp, creativeUnitDto.Width, creativeUnitDto.Height),
                                    Content = buffer,
                                    DocumentTypeId = 1
                                };
                                documentId = _documentService.Save(copyDoc);
                            }
                            returnFiles.Add(new UploadViewModel()
                            {
                                ParentId = creativeUnitDto.ID.ToString(),
                                TypeId = typeId.ToString(),
                                DocId = documentId
                            });
                        }
                    }

                }
            }
            return Json(returnFiles, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Remove()
        {
            // Return an empty string to signify success
            return Json(new { status = "OK" }, "text/plain");
        }

        #region Private Methods

        private FormatDto GetCreativeUnitFormat(IList<FormatDto> items, string fileExtension)
        {
            return items.FirstOrDefault(x => x.Format.Equals(fileExtension, StringComparison.OrdinalIgnoreCase));
        }

        #endregion

    }
}



