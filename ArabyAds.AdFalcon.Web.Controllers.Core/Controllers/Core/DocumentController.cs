using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using ArabyAds.AdFalcon.Domain.Common.Model.Campaign;
using ArabyAds.AdFalcon.Domain.Common.Model.Core;
using ArabyAds.AdFalcon.Domain.Common.Repositories.Campaign;
using ArabyAds.AdFalcon.Domain.Common.Repositories.Core;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Campaign.Creative;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Core;
using ArabyAds.AdFalcon.Services.Interfaces.Services.Campaign;
using ArabyAds.AdFalcon.Services.Interfaces.Services.Campaign.Creative;
using ArabyAds.AdFalcon.Services.Interfaces.Services.Core;
using System.Linq;
using ArabyAds.AdFalcon.Web.Controllers.Core;
using ArabyAds.AdFalcon.Web.Controllers.Model.Core;
using ArabyAds.AdFalcon.Web.Controllers.Utilities;
using System.Text;
using System.Drawing.Imaging;
using NReco.VideoConverter;

using ArabyAds.Framework;
using ArabyAds.AdFalcon.Services.Interfaces.Services.Account;
using System.IO.Compression;
using HtmlAgilityPack;
using System.Configuration;
using ArabyAds.Framework.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using ArabyAds.AdFalcon.Services.Interfaces.Messages;
using ArabyAds.AdFalcon.Web.Controllers.Model;

namespace ArabyAds.AdFalcon.Web.Controllers.Controllers.Core
{
    public class DocumentController : AuthorizedControllerBase
    {

        private IHostingEnvironment _hostingEnvironment;
        private readonly IDocumentService _documentService;
        private readonly IDocumentTypeService _documentTypeService;
        private ICreativeUnitService _creativeUnitService;
        private ITileImageService _tileImageService;
        protected IVideoTypeService _videoTypeService;
        protected ICampaignService _campaignService;

        protected IAccountService _accountService;
        public DocumentController(IHostingEnvironment hostingEnvironment)
        {

            _hostingEnvironment = hostingEnvironment;

            _tileImageService = IoC.Instance.Resolve<ITileImageService>();
            _documentService = IoC.Instance.Resolve<IDocumentService>();
            _documentTypeService = IoC.Instance.Resolve<IDocumentTypeService>();
            _creativeUnitService = IoC.Instance.Resolve<ICreativeUnitService>();
            _campaignService = IoC.Instance.Resolve<ICampaignService>();

            _accountService = IoC.Instance.Resolve<IAccountService>();
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
        //private static Id64Generator _id64Generator = new Id64Generator(int.Parse(JsonConfigurationManager.AppSettings["HostId"]));
        public ActionResult SaveTile(IFormFile attachment, int parentId)
        {
            var documentId = 0;
            var status = "OK";
            if (Request.Form.Files.Count == 0)
            {
                status = ResourcesUtilities.GetResource("NoFile", "Upload");
            }
            else
            {
                var fileToUpload = Request.Form.Files[0];

                if (fileToUpload.Length != 0)
                {

                    TileImageSizeDto sizeDTO = _tileImageService.GetSizeByParentId(new ValueMessageWrapper<int> { Value = parentId });
                    string extension = Path.GetExtension(fileToUpload.FileName).ToLower();

                    FormatDto format = sizeDTO.Formats.Where(p => p.Format == extension).SingleOrDefault();

                    if (format == null)
                    {
                        status = ResourcesUtilities.GetResource("FileType", "Upload");
                    }
                    else
                    {
                        var sourceImg = Image.FromStream(fileToUpload.OpenReadStream());

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
                                    Size =(int) fileToUpload.Length,
                                    Extension = Path.GetExtension(fileToUpload.FileName),
                                    Name = Path.GetFileNameWithoutExtension(fileToUpload.FileName),
                                    Content = buffer,
                                    DocumentTypeId = 1
                                };
                                documentId = _documentService.Save(doc).Value;
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
            return Json(new { status = status, DocumentId = documentId });
        }

        public ActionResult Save(IFormFile attachment, int? adTypeId, string group, int? parentId, int typeId, int AdSubTypeId = 0)
        {

            if (parentId==0)
            {

                parentId = null;
            }

            if (adTypeId == 0)
            {

                adTypeId = null;
            }
            var documentId = 0;
            var creativeUnitId = 0;
            int width, height;
            width = height = 0;
            var status = "OK";
            string FileName = string.Empty;
            string Extension = string.Empty;
            if (Request.Form.Files.Count == 0)
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
                if (AdSubTypeId == (int)ArabyAds.AdFalcon.Domain.Common.Model.Campaign.AdSubTypes.HTML5RichMedia || AdSubTypeId == (int)ArabyAds.AdFalcon.Domain.Common.Model.Campaign.AdSubTypes.HTML5Interstitial)
                {
                    typeIdvar = 0;
                }
                var creativeUnits = _creativeUnitService.GetByCriteriaWithouDeviceType(new GetCreativeUnitByCriteriaRequest {  CreativeUnitId= parentId, DeviceTypeId= (int)DeviceTypeEnum.Any , Group = group,  AdTypeId = adTypeId ,AdSubTypeId=AdSubTypeId>0?AdSubTypeId:(int?)null});
                if((typeIdvar>0))
                creativeUnits = creativeUnits.Where(M => M.DeviceType.ID == (int)typeIdvar || M.Code=="8").ToList();
                else
                    creativeUnits = creativeUnits.Where(M => M.DeviceType.ID == (int)DeviceTypeEnum.Any || M.DeviceType.ID == (int)typeIdvar).ToList();

                if (creativeUnits != null && creativeUnits.Count != 0)
                {
                  
                       var hpf = Request.Form.Files[0];
                    if (hpf.Length != 0)
                    {
                        using (var sourceImg = Image.FromStream(hpf.OpenReadStream()))
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
                                                Size = (int)hpf.Length,
                                                Extension = Path.GetExtension(hpf.FileName),
                                                Name = Path.GetFileNameWithoutExtension(hpf.FileName),
                                                Content = buffer,
                                                DocumentTypeId = _documentTypeService.GetByCode(format.Format).ID
                                            };
                                            documentId = _documentService.Save(doc).Value;
                                            status = "OK";
                                            FileName = Path.GetFileNameWithoutExtension(hpf.FileName);
                                            Extension = Path.GetExtension(hpf.FileName);

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
            return Json(new { status = status, DocumentId = documentId, CreativeUnitId = creativeUnitId, Width = width, Height = height  , FileExtension = Extension , FileName= FileName });
        }

        public ActionResult SaveForBulk(IFormFile attachment, int typeId, int? adTypeId)
        {
            var documentId = 0;
            var creativeUnitId = 0;
            int width, height;
            width = height = 0;
            var status = "OK";
            if (Request.Form.Files.Count == 0)
            {
                status = ResourcesUtilities.GetResource("NoFile", "Upload");
            }
            else if (Path.GetExtension(Request.Form.Files[0].FileName) == ".txt" || Path.GetExtension(Request.Form.Files[0].FileName) == ".TXT")
            {
                var PathName = Path.GetFileName(Request.Form.Files[0].FileName);
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
                    return Json(new { status = status, DocumentId = documentId, CreativeUnitId = creativeUnitId, Width = width, Height = height });
                }

                var creativeUnits = _creativeUnitService.GetByCriteriaWidthHeight( new GetCreativeUnitByCriteriaWithDimensionsRequest {  CreativeUnitId= null, DeviceTypeId= typeId, Group= "", AdTypeId= adTypeId, Width=   width,Height= height });

                var creativeUnit = creativeUnits.Where(p => (p.Width == width && p.Height == height) ||
                                                                 (p.HD_Width == width && p.HD_Height == height)).SingleOrDefault();

                if (creativeUnit != null)
                {
                    string fileContents;
                    var hpf = Request.Form.Files[0];
                    using (StreamReader reader = new StreamReader(hpf.OpenReadStream()))
                    {
                        status = "OK";
                        fileContents = reader.ReadToEnd();


                    }

                    return Json(new { status = status, fileContents = fileContents, CreativeUnitId = creativeUnitId, Width = width, Height = height });
                }

            }
            else
            {


                var hpf = Request.Form.Files[0];
                if (hpf.Length != 0)
                {
                    using (var sourceImg = Image.FromStream(hpf.OpenReadStream()))
                    {
                        var creativeUnits = _creativeUnitService.GetByCriteriaWidthHeight(new GetCreativeUnitByCriteriaWithDimensionsRequest { CreativeUnitId = null, DeviceTypeId = typeId, Group = "", AdTypeId = adTypeId, Width = sourceImg.Width, Height = sourceImg.Height });

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
                                            Size =(int) hpf.Length,
                                            Extension = Path.GetExtension(hpf.FileName),
                                            Name = Path.GetFileNameWithoutExtension(hpf.FileName),
                                            Content = buffer,
                                            DocumentTypeId = _documentTypeService.GetByCode(format.Format).ID
                                        };
                                        documentId = _documentService.Save(doc).Value;
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
            return Json(new { status = status, DocumentId = documentId, CreativeUnitId = creativeUnitId, Width = width, Height = height });
        }
        public ActionResult SaveForZip(IFormFile attachment, int typeId, int? adTypeId, int? AdSubTypeId)
        {
            var documentId = 0;
            var creativeUnitId = 0;
            int width, height;
            width = height = 0;
            var status = "OK";
            string FileName = string.Empty;
            Stream data = null;
            IList<ClickTagTrackerDto> clickTags = new List<ClickTagTrackerDto>();
            if (Request.Form.Files.Count == 0)
            {
                status = ResourcesUtilities.GetResource("NoFile", "Upload");
            }
            else if (Path.GetExtension(Request.Form.Files[0].FileName) != ".zip" && Path.GetExtension(Request.Form.Files[0].FileName) == ".ZIP")
            {
                status = ResourcesUtilities.GetResource("ZipFile", "Upload");


            }
            else
            {



                var hpf = Request.Form.Files[0];
                if (hpf.Length != 0)
                {
                    data = hpf.OpenReadStream(); // The original data
                    Stream unzippedEntryStream = null; // Unzipped data from a file in the archive
                    bool indexFound = false;
                    bool getParamaerrFunc = false;
                    bool ClickTagFunc = false;
                    int b = (int)hpf.Length;
                    decimal kb = (b / 1024);

                    if (kb > Config.SizeHTML5)
                    {
                        status = ResourcesUtilities.GetResource("FileSize", "Upload");
                        return Json(new { status = status, DocumentId = documentId, CreativeUnitId = creativeUnitId, Width = width, Height = height });
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
                            //    return Json(new { status = status, DocumentId = documentId, CreativeUnitId = creativeUnitId, Width = width, Height = height });
                            //}
                        }
                    }
                    if (indexFound == false)
                    {
                        status = ResourcesUtilities.GetResource("ZipFileIndex", "Upload");
                        return Json(new { status = status, DocumentId = documentId, CreativeUnitId = creativeUnitId, Width = width, Height = height });
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
                            return Json(new { status = status, DocumentId = documentId, CreativeUnitId = creativeUnitId, Width = width, Height = height });

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
                            return Json(new { status = status, DocumentId = documentId, CreativeUnitId = creativeUnitId, Width = width, Height = height });

                        }
                        if ((foundDuplicated))
                        {
                            //Invalid HTML

                            status = ResourcesUtilities.GetResource("ZipFileIndexDupClick", "Upload");
                            return Json(new { status = status, DocumentId = documentId, CreativeUnitId = creativeUnitId, Width = width, Height = height });

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
                    var creativeUnits = _creativeUnitService.GetByCriteriaWidthHeight(new GetCreativeUnitByCriteriaWithDimensionsRequest { CreativeUnitId=null, DeviceTypeId=0, Group="", AdTypeId=4,Width=width, Height= height });

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
                                    Size = (int)hpf.Length,
                                    Extension = Path.GetExtension(hpf.FileName),
                                    Name = Path.GetFileNameWithoutExtension(hpf.FileName),
                                    Content = buffer,
                                    DocumentTypeId = _documentTypeService.GetByCode(".zip").ID
                                };
                                documentId = _documentService.Save(doc).Value;
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
                            Size =(int) hpf.Length,
                            Extension = Path.GetExtension(hpf.FileName),
                            Name = Path.GetFileNameWithoutExtension(hpf.FileName),
                            Content = buffer,
                            DocumentTypeId = _documentTypeService.GetByCode(".zip").ID
                        };
                        documentId = _documentService.Save(doc).Value;
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
            return Json(new { status = status, DocumentId = documentId, CreativeUnitId = creativeUnitId, Width = width, Height = height, FileName = FileName, clickTags = clickTags });
        }


        public ActionResult SaveForCSV(IFormFile attachment, int AudienceListId, int DeviceTypeId)
        {
            var documentId = 0;
            
     
            var status = "OK";
            string FileName = string.Empty;
            Stream data = null;
           
            if (Request.Form.Files.Count == 0)
            {
                status = ResourcesUtilities.GetResource("NoFile", "Upload");
            }
            else if (!((Path.GetExtension(Request.Form.Files[0].FileName) == ".csv" || Path.GetExtension(Request.Form.Files[0].FileName) == ".CSV" ) || (Path.GetExtension(Request.Form.Files[0].FileName) == ".txt" || Path.GetExtension(Request.Form.Files[0].FileName) == ".TXT")))
            {
                status = ResourcesUtilities.GetResource("CSVFile", "Upload");


            }
            else
            {



                var hpf = Request.Form.Files[0];
                if (hpf.Length != 0)
                {
                    data = hpf.OpenReadStream(); // The original data
                 
                    int b = (int)hpf.Length;
                    decimal kb = (b / 1024);

                    if (kb > Config.SizeCSV)
                    {
                        status = ResourcesUtilities.GetResource("FileSize", "Upload");
                        return Json(new { status = status, DocumentId = documentId});
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
                    
                    var uploadFilesDir = JsonConfigurationManager.AppSettings["DeviceIdFilesPath"];

                    if (!Directory.Exists(uploadFilesDir))
                    {
                        Directory.CreateDirectory(uploadFilesDir);

                    }
                    //throw new Exception();
                    
                    var fileSavePath = Path.Combine(uploadFilesDir, FileName);

                    //hpf.SaveAs(fileSavePath);

                    using (var fileStream = System.IO.File.Create(fileSavePath))
                    {
                        var sream = hpf.OpenReadStream();
                        sream.Seek(0, SeekOrigin.Begin);
                        sream.CopyTo(fileStream);
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
            return Json(new { status = status, DocumentId = documentId,  FileName = FileName });
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
          
                var documentDto = _documentService.Get(new ValueMessageWrapper<int> { Value = documentId });
                byte[] fileBytes = documentDto.Content;
                string fileName = documentDto.Name/*+documentDto.Extension*/;
                return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
            
          
        }
        [RequireHttps]
        public ActionResult SaveHttps(IFormFile attachment)
        {
            var documentId = 0;
            var creativeUnitId = 0;
            int width, height;
            width = height = 0;
            var status = "OK";
            if (Request.Form.Files.Count == 0)
            {
                status = ResourcesUtilities.GetResource("NoFile", "Upload");
            }
            else
            {
                var hpf = Request.Form.Files[0];
                if (hpf.Length != 0)
                {
                    using (var sourceImg = Image.FromStream(hpf.OpenReadStream()))
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
                                Size =(int) hpf.Length,
                                Extension = Path.GetExtension(hpf.FileName),
                                Name = Path.GetFileNameWithoutExtension(hpf.FileName),
                                Content = buffer,
                                DocumentTypeId = _documentTypeService.GetByCode(".png").ID
                            };
                            documentId = _documentService.Save(doc).Value;
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
            return Json(new { status = status, DocumentId = documentId, CreativeUnitId = creativeUnitId, Width = width, Height = height, onchange = "onImageChanged" });
        }

        public ActionResult SaveStreamVideo(IFormFile attachment, int? adTypeId, string group, int? parentId, int typeId)
        {

            var documentId = 0;
            int thumbnailDocId = -1;
            var creativeUnitId = 0;
            var status = "OK";
            var imgThumbnail = string.Empty;
            var videoInformation = new VideoInformationModel();



            try
            {
                if (Request.Form.Files.Count == 0)
                {
                    status = ResourcesUtilities.GetResource("NoFile", "Upload");
                }
                else
                {
                    var creativeUnits = _creativeUnitService.GetBy(   new GetCreativeUnitRequest {  DeviceType= (DeviceTypeEnum)typeId, AdType= AdTypeIds.InStreamVideo, AdSubType= AdSubTypes.VideoLinear,Group= group }).ToList();
                     

                    if (creativeUnits != null && creativeUnits.Count != 0)
                    {


                        var hpf = Request.Form.Files[0];
                        if (hpf.Length != 0)
                        {
                            var videoType = _campaignService.GetMIMEType("video/" + Path.GetExtension(hpf.FileName).Replace(".", "").Trim()).Value;
                            if (videoType >0)
                            {
                                PathProvider pathprovider = new PathProvider(_hostingEnvironment);
                                
                                  var uploadFilesDir = pathprovider.MapPath("UploadedVideos");

                                if (!Directory.Exists(uploadFilesDir))
                                {
                                    Directory.CreateDirectory(uploadFilesDir);

                                }
                                //throw new Exception();

                                var fileSavePath = Path.Combine(uploadFilesDir, hpf.FileName);

                                //hpf.SaveAs(fileSavePath);
                                using (var fileStream = System.IO.File.Create(fileSavePath))
                                {
                                    var sream = hpf.OpenReadStream();
                                    sream.Seek(0, SeekOrigin.Begin);
                                    sream.CopyTo(fileStream);
                                }
                                videoInformation = GetVideoInformation(fileSavePath);
                                if (videoInformation.Duration <= (Config.InstreamVideoDuraionLimit))
                                {
                                    int frameWidth = videoInformation.Width;
                                    int frameHeight = videoInformation.Height;
                                    videoInformation.FormatName = "video/" + Path.GetExtension(hpf.FileName).Replace(".", "").Trim();
                                    var OrginalcreativeUnit = _creativeUnitService.GetById(new ValueMessageWrapper<int> { Value = 21 });
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
                                            var buffer = new byte[Request.Form.Files[0].Length];
                                            hpf.OpenReadStream().Read(buffer, 0, (int)Request.Form.Files[0].Length);


                                            if (buffer.Length > format.MaxSize)
                                            {
                                                status = string.Format(ResourcesUtilities.GetResource("FileSize1", "Upload"), format.MaxSize / 1024);
                                            }
                                            else
                                            {


                                                var doc = new DocumentDto
                                                {
                                                    Size =(int) hpf.Length,
                                                    Extension = Path.GetExtension(hpf.FileName),
                                                    Name = Path.GetFileNameWithoutExtension(hpf.FileName),
                                                    //  Content = 
                                                    DocumentTypeId = _documentTypeService.GetByCode(Request.Form.Files[0].ContentType).ID
                                                };
                                                doc.Content = buffer;// new byte[Request.Form.Files[0].Length];
                                                //  hpf.OpenReadStream().ReadAsync(doc.Content, 0, Request.Form.Files[0].Length);
                                                doc.IsWebHDFS = true;
                                                var subFolder = Framework.Utilities.Environment.GetServerTime().ToString("yyyyMMdd");
                                            
                                               //doc.Name = doc.Name + doc.Extension;
                                                documentId = _documentService.Save(doc).Value;
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

                    videoName = Request.Form.Files[0].FileName
                });


            }




            return Json(new
            {
                img = imgThumbnail,
                status = status,
                VideoDocumentId = documentId,
                ThumbnailDocId = thumbnailDocId,
                CreativeUnitId = creativeUnitId,
                VideoInformation = videoInformation,

                videoName = Request.Form.Files[0].FileName
            });
        }

        private void SaveVideoThumbnail(string pathToVideoFile, out int docId)
        {

            DocumentUtility.SaveVideoThumbnail(pathToVideoFile, out docId);

        }

        public FileResult DownloadTaxDocument(int documentId)
        {
          var checkResult =  _accountService.CheckIfDocumentbelongToAccount(new ValueMessageWrapper<int> { Value = documentId }).Value;

            if (checkResult)
            {
                var documentDto = _documentService.Get(new ValueMessageWrapper<int> { Value = documentId });
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
        public ActionResult SaveAttachment(IFormFile attachment, int typeId)
        {
            var documentId = 0;
            var documentName = string.Empty;
            var status = "OK";
            if (Request.Form.Files.Count == 0)
            {
                status = ResourcesUtilities.GetResource("NoFile", "Upload");
            }
            else
            {
                //get attachment unit
                var hpf = Request.Form.Files[0];
                if (hpf.Length != 0)
                {
                    {
                        //read file content and get file as bytes
                        var buffer = ReadFully(hpf.OpenReadStream(),(int) hpf.Length);
                        var doc = new DocumentDto
                        {
                            Size =(int) hpf.Length,
                            Extension = Path.GetExtension(hpf.FileName),
                            Name = Path.GetFileNameWithoutExtension(hpf.FileName),
                            Content = buffer,
                            DocumentTypeId = typeId
                        };
                        documentId = _documentService.Save(doc).Value;
                        documentName = doc.Name;
                        status = "OK";
                    }
                }
            }
            return Json(new { status = status, DocumentId = documentId, DocumentName = documentName });
        }

        [RequireHttps(Order = 1)]
        public ActionResult RemoveHttps()
        {
            // Return an empty string to signify success
            return Json(new { status = "OK", onchange = "onImageChanged" });
        }


       [HttpPost]
        public ActionResult Copy([FromBody]DocumentUploadModel modelDoc)
        {
          int  documentId = modelDoc.documentId;
            int parentId = modelDoc.parentId;

            int typeId = modelDoc.typeId;
            int adTypeId = modelDoc.adTypeId;

            AdTypeIds adType = (AdTypeIds)adTypeId;
            var returnFiles = new List<UploadViewModel>();
            var doc = _documentService.Get(new ValueMessageWrapper<int> { Value = documentId });

            if (doc != null)
            {
                using (var sourceImg = ByteArrayToImage(doc.Content))
                {
                    var creativeUnits = _creativeUnitService.GetBy( new GetCreativeUnitRequest { DeviceType= (DeviceTypeEnum)typeId,  AdType=adType,  AdSubType=null, Group= null });
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
                                documentId = _documentService.Save(copyDoc).Value;
                            }
                            returnFiles.Add(new UploadViewModel()
                            {
                                ParentId = creativeUnitDto.ID.ToString(),
                                TypeId = typeId.ToString(),
                                DocId = documentId
                            });
                        }
                    }
                    var creativeUnitAnys = _creativeUnitService.GetBy(  new GetCreativeUnitRequest {  DeviceType=DeviceTypeEnum.Any,  AdType=adType, AdSubType= null, Group= null } );
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
                                documentId = _documentService.Save(copyDoc).Value;
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
            return Json(returnFiles);
        }
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Remove()
        {
            // Return an empty string to signify success
            return Json(new { status = "OK" });
        }

        #region Private Methods

        private FormatDto GetCreativeUnitFormat(IList<FormatDto> items, string fileExtension)
        {
            return items.FirstOrDefault(x => x.Format.Equals(fileExtension, StringComparison.OrdinalIgnoreCase));
        }

        #endregion

    }
}



