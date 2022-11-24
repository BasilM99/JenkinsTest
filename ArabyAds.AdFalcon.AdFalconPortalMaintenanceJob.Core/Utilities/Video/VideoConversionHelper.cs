using ArabyAds.AdFalcon.Domain.Common.Model.Account;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Account;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Account.Fund;
using ArabyAds.AdFalcon.Services.Interfaces.Services.Account.Fund;
using ArabyAds.AdFalcon.Services.Interfaces.Services.Campaign;
using ArabyAds.AdFalcon.Services.Interfaces.Services.Campaign.Creative;
using ArabyAds.AdFalcon.Services.Interfaces.Services.Core;
using ArabyAds.Framework;
using ArabyAds.Framework.ExceptionHandling;
using ArabyAds.Framework.ExceptionHandling.Exceptions;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using ArabyAds.AdFalcon.Domain.Common.Model.Core;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Core;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Campaign;
using FFMpegCore;
using FFMpegCore.Enums;
using ArabyAds.AdFalcon.Services.Interfaces.Messages;
using ArabyAds.AdFalcon.Domain.Common.Model.Campaign;
using System.Drawing;
using ArabyAds.Framework.Utilities;
//using MediaToolkit.Model;
//using MediaToolkit;

namespace ArabyAds.AdFalcon.AdFalconPortalMaintenanceJob.Utilities.Video
{
    public class VideoConversionHelper
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
            FilesPath = JsonConfigurationManager.AppSettings["VideoFolderCreation"];

            if (!string.IsNullOrEmpty(JsonConfigurationManager.AppSettings["VideoAudioBitRate"]))
                defaultAudioBitRate = Convert.ToInt32(JsonConfigurationManager.AppSettings["VideoAudioBitRate"]);
            if (!string.IsNullOrEmpty(JsonConfigurationManager.AppSettings["VideoFPS"]))
                defaulVideoFPS = Convert.ToDouble(JsonConfigurationManager.AppSettings["VideoFPS"]);
            if (!string.IsNullOrEmpty(JsonConfigurationManager.AppSettings["lagestFileSize"]))
                lagestFileSize = Convert.ToInt32(JsonConfigurationManager.AppSettings["lagestFileSize"]);



        }

    
        public void ResolveDraftVideos()
        {

            ApplicationContext.Instance.Logger.Info("Start Video Resolve");

            // get  Draft Video. 
            var draftVideos = _CampaignService.GetDraftVideoAd();

            var videosCreativeUnits = _CreativeUnitService.GetBy(new GetCreativeUnitRequest { DeviceType = DeviceTypeEnum.Any, AdType = AdTypeIds.InStreamVideo, AdSubType = AdSubTypes.VideoLinear });

            foreach (var drftVideo in draftVideos)
            {
                try
                {
                    try
                    {
                        string test = JsonConfigurationManager.AppSettings["VideoFolderCreation"];
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




                    if (!drftVideo.CreativeUnitsContent[0].DocumentId.HasValue) continue;
                    var docDto = _DocumentService.Get(ValueMessageWrapper.Create(drftVideo.CreativeUnitsContent[0].DocumentId.Value));


                    //drftVideo.CreativeUnitsContent[0].InStreamVideoCreativeUnit.OriginalCreativeUnitID = 104;
                    //drftVideo.CreativeUnitsContent[0].InStreamVideoCreativeUnit.Code =""+ 80;
                    var inputFilePath = FilesPath + drftVideo.CreativeUnitsContent[0].InStreamVideoCreativeUnit.OriginalCreativeUnitID + "_Convert" + docDto.Extension;
                    File.WriteAllBytes(inputFilePath, docDto.Content);
                    var inputFileAnalysis = FFProbe.Analyse(inputFilePath);

                    var stringGroupCode = _CreativeUnitService.GetGroupByCreativeByID(ValueMessageWrapper.Create(drftVideo.CreativeUnitsContent[0].InStreamVideoCreativeUnit.OriginalCreativeUnitID));

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
                            var outputFilePath = FilesPath + relatedCreativeUnit.ID + docDto.Extension;
                            IMediaAnalysis outputFileAnalysis;
                            if (drftVideo.CreativeUnitsContent[0].InStreamVideoCreativeUnit.OriginalCreativeUnitID == relatedCreativeUnit.ID)
                            {


                                if (docDto.Size <= relatedCreativeUnit.Formats[0].MaxSize)
                                {

                                    File.WriteAllBytes(FilesPath + drftVideo.CreativeUnitsContent[0].InStreamVideoCreativeUnit.OriginalCreativeUnitID + docDto.Extension, docDto.Content);

                                    originalFilePath = FilesPath + drftVideo.CreativeUnitsContent[0].InStreamVideoCreativeUnit.OriginalCreativeUnitID + docDto.Extension;


                                    MatchOrginal = true;
                                    outputFilePath = originalFilePath;

                                }
                            }

                            int DocId = 0;
                            var videoCrvAds = ConfigrelatedCreativeUnit;
                            var frameRate = inputFileAnalysis.PrimaryVideoStream.FrameRate;
                            long? audioBitRate = inputFileAnalysis.PrimaryAudioStream?.BitRate;
                            if (frameRate > videoCrvAds.VideoFrameRate)
                            {
                                frameRate = videoCrvAds.VideoFrameRate;
                            }
                            if (audioBitRate.HasValue && audioBitRate > videoCrvAds.AudioBitRate * 1024)
                            {
                                audioBitRate = videoCrvAds.AudioBitRate;
                            }
                            else
                            {
                                audioBitRate = audioBitRate / 1024; 
                            
                            }

                            if (!MatchOrginal)
                            {
                                if (audioBitRate.HasValue)
                                {
                                    var fFMpegArguments = FFMpegArguments
    .FromFileInput(inputFilePath)
    .OutputToFile(outputFilePath, true, options => options
        .WithVideoBitrate(ConfigrelatedCreativeUnit.BitRate)
        .WithFramerate(frameRate)
        .WithAudioBitrate((int)audioBitRate.Value)
        //.WithVariableBitrate(4)
        .WithVideoFilters(filterOptions => filterOptions
            .Scale(relatedCreativeUnit.Width, relatedCreativeUnit.Height))
        .WithFastStart())
    .ProcessSynchronously();
                                }

                                else
                                {
                                    var fFMpegArguments = FFMpegArguments
    .FromFileInput(inputFilePath)
    .OutputToFile(outputFilePath, true, options => options
        .WithVideoBitrate(ConfigrelatedCreativeUnit.BitRate)
        .WithFramerate(frameRate)
        //.WithAudioBitrate(audioBitRate.Value)
        //.WithVariableBitrate(4)
        .WithVideoFilters(filterOptions => filterOptions
            .Scale(relatedCreativeUnit.Width, relatedCreativeUnit.Height))
        .WithFastStart())
    .ProcessSynchronously();
                                }



                                /* var fFMpegArguments = FFMpegArguments
                                 .FromInputFiles(inputFilePath)
                                 .Scale(relatedCreativeUnit.Width, relatedCreativeUnit.Height)
                                 .WithVideoBitrate(ConfigrelatedCreativeUnit.BitRate)
                                 .WithFramerate(frameRate);

                                 if (audioBitRate.HasValue)
                                     fFMpegArguments = fFMpegArguments.WithAudioBitrate(audioBitRate.Value);


                                 fFMpegArguments.WithFastStart()
                                 .OutputToFile(outputFilePath)
                                 .ProcessSynchronously();*/


                                outputFileAnalysis = FFProbe.Analyse(outputFilePath);

                                var fileInfo = new FileInfo(outputFilePath);
                                DocId = CreateDocument(outputFilePath, docDto);
                            }
                            else
                            {
                                var fileInfo = new System.IO.FileInfo(originalFilePath);
                                if (lagestFileSize < fileInfo.Length)
                                {

                                    if (audioBitRate.HasValue)
                                    {
                                        var fFMpegArguments = FFMpegArguments
        .FromFileInput(inputFilePath)
        .OutputToFile(outputFilePath, true, options => options
            .WithVideoBitrate(ConfigrelatedCreativeUnit.BitRate)
            .WithFramerate(frameRate)
            .WithAudioBitrate((int)audioBitRate.Value)
            //.WithVariableBitrate(4)
            .WithVideoFilters(filterOptions => filterOptions
                .Scale(relatedCreativeUnit.Width, relatedCreativeUnit.Height))
            .WithFastStart())
        .ProcessSynchronously();
                                    }

                                    else
                                    {
                                        var fFMpegArguments = FFMpegArguments
        .FromFileInput(inputFilePath)
        .OutputToFile(outputFilePath, true, options => options
            .WithVideoBitrate(ConfigrelatedCreativeUnit.BitRate)
            .WithFramerate(frameRate)
            //.WithAudioBitrate(audioBitRate.Value)
            //.WithVariableBitrate(4)
            .WithVideoFilters(filterOptions => filterOptions
                .Scale(relatedCreativeUnit.Width, relatedCreativeUnit.Height))
            .WithFastStart())
        .ProcessSynchronously();
                                    }



                                   /* var fFMpegArguments = FFMpegArguments
                              .FromInputFiles(inputFilePath)
                              .Scale(relatedCreativeUnit.Width, relatedCreativeUnit.Height)
                              .WithVideoBitrate(ConfigrelatedCreativeUnit.BitRate)
                              .WithFramerate(frameRate);

                                    if (audioBitRate.HasValue)
                                        fFMpegArguments = fFMpegArguments.WithAudioBitrate(audioBitRate.Value);


                                    fFMpegArguments.WithFastStart()
                                    .OutputToFile(outputFilePath)
                                   // .NotifyOnProgress()
                                    .ProcessSynchronously();*/
                                }
                                outputFileAnalysis = FFProbe.Analyse(outputFilePath);

                                DocId = CreateDocument(originalFilePath, docDto);

                                if (docDto.Size > relatedCreativeUnit.Formats[0].MaxSize)
                                {
                                    continue;
                                }
                            }

                            //else
                            //{
                            //    conversionOptions.AudioBitRate = inputFile.Metadata.AudioData.BitRateKbs;
                            //}
                            

                            var dto = new VideoMediaFileDto();
                            dto.VideoAdId = drftVideo.ID;
                            dto.CreativeUnitId = relatedCreativeUnit.ID;
                            dto.width = relatedCreativeUnit.Width;
                            dto.height = relatedCreativeUnit.Height;
                            dto.DocumentId = DocId;
                            dto.bitRate =(int) outputFileAnalysis.PrimaryVideoStream.BitRate/1024;
                            dto.VideoTypeId = _DocumentTypeService.GetMIMEById(ValueMessageWrapper.Create(docDto.DocumentTypeId)).Value;
                            dto.DeliveryMethodId = drftVideo.CreativeUnitsContent[0].InStreamVideoCreativeUnit.DeliveryMethod;
                            dto.AdCreativeUnitId = drftVideo.CreativeUnitsContent[0].ID;



                            _CampaignService.AddUpdateVideoMediaFile(dto);
                            puplishvideoAd = true;
                        }



                        catch (Exception ex)
                        {
                            Framework.ApplicationContext.Instance.Logger.Error(ex.Message, ex);

                        }
                    }
                    if (puplishvideoAd)
                        _CampaignService.PublishVideoAd(ValueMessageWrapper.Create(drftVideo.ID));

                }

                catch (Exception ex)
                {
                    Framework.ApplicationContext.Instance.Logger.Error(ex.Message, ex);

                }

            }

            ApplicationContext.Instance.Logger.Info("Finish Video Resolve");




        }


        public int CreateDocument(string inputPath, DocumentDto dto)
        {
            DocumentDto inputDocumentDto = new DocumentDto();
            //inputDocumentDto.Content = File.ReadAllBytes(inputPath);

            inputDocumentDto.InputPath = inputPath;
            inputDocumentDto.DocumentTypeId = dto.DocumentTypeId;
            inputDocumentDto.Extension = dto.Extension;
            FileInfo fileInfo = new FileInfo(inputPath);
            inputDocumentDto.Name = fileInfo.Name.Replace(fileInfo.Extension, string.Empty);
            inputDocumentDto.Size = (int)fileInfo.Length;

            int DocId = _DocumentService.SaveFromInputPath(inputDocumentDto).Value;

            return DocId;

        }
    }
}
