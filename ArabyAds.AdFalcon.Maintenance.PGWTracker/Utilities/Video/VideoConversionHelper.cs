using Noqoush.AdFalcon.Domain.Common.Model.Account;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Account;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Account.Fund;
using Noqoush.AdFalcon.Services.Interfaces.Services.Account.Fund;
using Noqoush.AdFalcon.Services.Interfaces.Services.Campaign;
using Noqoush.AdFalcon.Services.Interfaces.Services.Campaign.Creative;
using Noqoush.AdFalcon.Services.Interfaces.Services.Core;
using Noqoush.Framework;
using Noqoush.Framework.ExceptionHandling;
using Noqoush.Framework.ExceptionHandling.Exceptions;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Noqoush.AdFalcon.Domain.Common.Model.Core;
using MediaToolkit.Model;
using MediaToolkit;
using MediaToolkit.Options;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Core;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Campaign;

namespace Noqoush.AdFalcon.AdFalconPortalMaintenanceJob.Utilities.Video
{
  public  class VideoConversionHelper
    {
        private ICreativeUnitService _CreativeUnitService;
        private IDocumentService _DocumentService;
        private ICampaignService _CampaignService;
        private IVideoTypeService _VideoTypeService;
        private IDocumentTypeService _DocumentTypeService;
        //private IVideoConversionCreativeUnitRepository _VideoConversionCreativeUnitRepository;

        private string FilesPath;
        private double defaulVideoFPS = 30;
        private int defaultAudioBitRate = 128;

        private int lagestFileSize = 26214400;
        public VideoConversionHelper()
        {

            _CreativeUnitService = IoC.Instance.Resolve<ICreativeUnitService>();
            _DocumentService = IoC.Instance.Resolve<IDocumentService>();
            _CampaignService = IoC.Instance.Resolve<ICampaignService>();
            _VideoTypeService = IoC.Instance.Resolve<IVideoTypeService>();
            _DocumentTypeService = IoC.Instance.Resolve<IDocumentTypeService>();
            //_VideoConversionCreativeUnitRepository = IoC.Instance.Resolve<IVideoConversionCreativeUnitRepository>();
            FilesPath = System.Configuration.ConfigurationManager.AppSettings["VideoFolderCreation"];

            if (!string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["VideoAudioBitRate"]))
                defaultAudioBitRate = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["VideoAudioBitRate"]);
            if (!string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["VideoFPS"]))
                defaulVideoFPS = Convert.ToDouble(System.Configuration.ConfigurationManager.AppSettings["VideoFPS"]);
            if (!string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["lagestFileSize"]))
                lagestFileSize = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["lagestFileSize"]);

            

        }

        private void engine_ConvertProgressEvent(object sender, ConvertProgressEventArgs e)
        {

            ApplicationContext.Instance.Logger.Info(string.Format("Bitrate: {0}", e.Bitrate));
            ApplicationContext.Instance.Logger.Info(string.Format("Fps: {0}", e.Fps));
            ApplicationContext.Instance.Logger.Info(string.Format("Frame: {0}", e.Frame));
            ApplicationContext.Instance.Logger.Info(string.Format("ProcessedDuration: {0}", e.ProcessedDuration));
            ApplicationContext.Instance.Logger.Info(string.Format("SizeKb: {0}", e.SizeKb));
            ApplicationContext.Instance.Logger.Info(string.Format("TotalDuration: {0}\n", e.TotalDuration));
        }

        private void engine_ConversionCompleteEvent(object sender, ConversionCompleteEventArgs e)
        {


            ApplicationContext.Instance.Logger.Info("\n------------\nConversion complete!\n------------");
            ApplicationContext.Instance.Logger.Info(string.Format("Bitrate: {0}", e.Bitrate));
            ApplicationContext.Instance.Logger.Info(string.Format("Fps: {0}", e.Fps));
            ApplicationContext.Instance.Logger.Info(string.Format("Frame: {0}", e.Frame));
            ApplicationContext.Instance.Logger.Info(string.Format("ProcessedDuration: {0}", e.ProcessedDuration));
            ApplicationContext.Instance.Logger.Info(string.Format("SizeKb: {0}", e.SizeKb));
            ApplicationContext.Instance.Logger.Info(string.Format("TotalDuration: {0}\n", e.TotalDuration));
        }
        public void ResolveDraftVideos()
        {
            
                ApplicationContext.Instance.Logger.Info("Start Video Resolve");

                // get  Draft Video. 
                var draftVideos = _CampaignService.GetDraftVideoAd();
                
               var videosCreativeUnits= _CreativeUnitService.GetBy(DeviceTypeEnum.Any, Domain.Common.Model.Campaign.AdTypeIds.InStreamVideo, Domain.Common.Model.Campaign.AdSubTypes.VideoLinear, null);
                
                foreach (var drftVideo in draftVideos)
                {
                try
                {
                    try
                    {
                        string test = System.Configuration.ConfigurationManager.AppSettings["VideoFolderCreation"];
                        string[] filePaths = Directory.GetFiles(test);
                        foreach (string filePath in filePaths)
                        {

                            File.Delete(filePath);

                        }
                    }
                    catch (Exception ex)
                    {
                        Framework.ApplicationContext.Instance.Logger.Error(ex.Message, ex);

                    }





                    var docDto = _DocumentService.Get(drftVideo.CreativeUnitsContent[0].DocumentId.Value);


                    //drftVideo.CreativeUnitsContent[0].InStreamVideoCreativeUnit.OriginalCreativeUnitID = 104;
                    //drftVideo.CreativeUnitsContent[0].InStreamVideoCreativeUnit.Code =""+ 80;

                    File.WriteAllBytes(FilesPath + drftVideo.CreativeUnitsContent[0].InStreamVideoCreativeUnit.OriginalCreativeUnitID + "_Convert" + docDto.Extension, docDto.Content);


                    var inputFile = new MediaFile { Filename = FilesPath + drftVideo.CreativeUnitsContent[0].InStreamVideoCreativeUnit.OriginalCreativeUnitID + "_Convert" + docDto.Extension };
                    using (var engine = new Engine())
                    {

                        engine.GetMetadata(inputFile);
                    }
                    VideoMediaFileDto dto = new VideoMediaFileDto();
                    /* dto.VideoAdId = drftVideo.ID;
                     dto.CreativeUnitId = +drftVideo.CreativeUnitsContent[0].InStreamVideoCreativeUnit.OriginalCreativeUnitID;
                     dto.width = drftVideo.CreativeUnitsContent[0].CreativeUnit.Width;
                     dto.height = drftVideo.CreativeUnitsContent[0].CreativeUnit.Height;
                     dto.DocumentId = drftVideo.CreativeUnitsContent[0].DocumentId.Value;
                     dto.bitRate = inputFile.Metadata.VideoData.BitRateKbs.Value;
                     dto.VideoTypeId= drftVideo.CreativeUnitsContent[0].InStreamVideoCreativeUnit.VideoType;
                     dto.DeliveryMethodId = drftVideo.CreativeUnitsContent[0].InStreamVideoCreativeUnit.DeliveryMethod;
                     dto.AdCreativeUnitId = drftVideo.CreativeUnitsContent[0].ID;
                     _CampaignService.AddUpdateVideoMediaFile(dto);*/
                    //docDto.sa
                    var stringGroupCode = _CreativeUnitService.GetGroupByCreativeByID(drftVideo.CreativeUnitsContent[0].InStreamVideoCreativeUnit.OriginalCreativeUnitID);

                    var creativUnitsReleted = _CreativeUnitService.GetByGroupCode(stringGroupCode);

                    var RelatedCreativeUnits = creativUnitsReleted.Where(M => M.Width <= drftVideo.CreativeUnitsContent[0].InStreamVideoCreativeUnit.VideoWidth /*&& M.ID!= drftVideo.CreativeUnitsContent[0].InStreamVideoCreativeUnit.OriginalCreativeUnitID*/).ToList();

                    var VideosReleated = _CampaignService.GetVideoConversionCreativeUnits(stringGroupCode);
                    var MatchOrginal = false;
                    var puplishvideoAd = false;
                    string originalFilePath = string.Empty;
                    foreach (var ConfigrelatedCreativeUnit in VideosReleated)
                    {
                        MatchOrginal = false;
                        puplishvideoAd = false;

                        try
                        {


                            var relatedCreativeUnit = RelatedCreativeUnits.Where(M => M.ID == ConfigrelatedCreativeUnit.CreativeUnitId).SingleOrDefault();

                            if (relatedCreativeUnit == null)
                                continue;
                            var outputFile = new MediaFile { Filename = FilesPath + relatedCreativeUnit.ID + docDto.Extension };
                            if (drftVideo.CreativeUnitsContent[0].InStreamVideoCreativeUnit.OriginalCreativeUnitID == relatedCreativeUnit.ID)
                            {


                                if (docDto.Size <= relatedCreativeUnit.Formats[0].MaxSize)
                                {

                                    File.WriteAllBytes(FilesPath + drftVideo.CreativeUnitsContent[0].InStreamVideoCreativeUnit.OriginalCreativeUnitID + docDto.Extension, docDto.Content);

                                    originalFilePath = FilesPath + drftVideo.CreativeUnitsContent[0].InStreamVideoCreativeUnit.OriginalCreativeUnitID + docDto.Extension;
                                 

                                    MatchOrginal = true;
                                    outputFile = new MediaFile { Filename = originalFilePath };

                                }
                            }
                            // var outputFile = new MediaFile { Filename = FilesPath + relatedCreativeUnit.ID + docDto.Extension };
                            //if (relatedCreativeUnit.ID == 104)
                            //{
                            //    outputFile.Filename = @"C:\AdFalcon\VideoFiles\114.mp4";
                            //   // conversionOptions.VideoBitRate = 1200;
                            //}
                            using (var engine = new Engine())
                            {
                                //engine.ConvertProgressEvent += engine_ConvertProgressEvent;
                                //engine.ConversionCompleteEvent += engine_ConversionCompleteEvent;
                                var conversionOptions = new ConversionOptions { VideoSize = VideoSize.Custom, CustomHeight = relatedCreativeUnit.Height, CustomWidth = relatedCreativeUnit.Width, VideoBitRate = ConfigrelatedCreativeUnit.BitRate };

                                int DocId = 0;

                                var videoCrvAds = ConfigrelatedCreativeUnit;


                                if (inputFile.Metadata.VideoData.Fps > videoCrvAds.VideoFrameRate)
                                {

                                    conversionOptions.VideoFps = videoCrvAds.VideoFrameRate;

                                }
                                if (inputFile.Metadata.AudioData!=null   &&( inputFile.Metadata.AudioData.BitRateKbs > videoCrvAds.AudioBitRate))
                                {

                                    conversionOptions.AudioBitRate = videoCrvAds.AudioBitRate;
                                   
                                }
                                //else
                                //{
                                //    conversionOptions.AudioBitRate = inputFile.Metadata.AudioData.BitRateKbs;

                                //}


                                if (!MatchOrginal)
                                {

                                    engine.Convert(inputFile, outputFile, conversionOptions);
                                    engine.GetMetadata(outputFile);
                                    Int64 fileSizeInBytes = new FileInfo(outputFile.Filename).Length;
                                    // int DocId = 0;
                                    DocId = CreateDocument(outputFile.Filename, docDto);
                                }
                                else
                                {
                                    long length = new System.IO.FileInfo(originalFilePath).Length;
                                    if (lagestFileSize< length)
                                    engine.Convert(inputFile, outputFile, conversionOptions);
                                    engine.GetMetadata(outputFile);

                                    DocId = CreateDocument(originalFilePath, docDto);

                                    if (docDto.Size > relatedCreativeUnit.Formats[0].MaxSize)
                                    {
                                        continue;
                                    }
                                }




                                dto = new VideoMediaFileDto();
                                dto.VideoAdId = drftVideo.ID;
                                dto.CreativeUnitId = relatedCreativeUnit.ID;
                                dto.width = relatedCreativeUnit.Width;
                                dto.height = relatedCreativeUnit.Height;
                                dto.DocumentId = DocId;
                                dto.bitRate = outputFile.BitRateKbs;
                                dto.VideoTypeId = _DocumentTypeService.GetMIMEById(docDto.DocumentTypeId);
                                dto.DeliveryMethodId = drftVideo.CreativeUnitsContent[0].InStreamVideoCreativeUnit.DeliveryMethod;
                                dto.AdCreativeUnitId = drftVideo.CreativeUnitsContent[0].ID;



                                _CampaignService.AddUpdateVideoMediaFile(dto);
                                puplishvideoAd = true;
                            }


                        }
                        catch (Exception ex)
                        {
                            Framework.ApplicationContext.Instance.Logger.Error(ex.Message, ex);

                        }
                    }
                    if(puplishvideoAd)
                    _CampaignService.PublishVideoAd(drftVideo.ID);

                }

                catch (Exception ex)
                {
                    Framework.ApplicationContext.Instance.Logger.Error(ex.Message, ex);

                }
              
                }

           ApplicationContext.Instance.Logger.Info("Finish Video Resolve");

         
          

        }


        public int CreateDocument(string inputPath,DocumentDto dto )
        {
            DocumentDto inputDocumentDto = new DocumentDto();
            //inputDocumentDto.Content = File.ReadAllBytes(inputPath);

            inputDocumentDto.InputPath = inputPath;
            inputDocumentDto.DocumentTypeId = dto.DocumentTypeId;
            inputDocumentDto.Extension = dto.Extension;
            FileInfo fileInfo = new FileInfo(inputPath);
            inputDocumentDto.Name = fileInfo.Name.Replace(fileInfo.Extension,string.Empty);
            inputDocumentDto.Size =(int) fileInfo.Length;
     
            int DocId=_DocumentService.SaveFromInputPath(inputDocumentDto);

            return DocId;

        }
    }
}
