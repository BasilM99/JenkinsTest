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

using Noqoush.AdFalcon.Server.Integration.Services;
namespace Noqoush.AdFalcon.AdFalconPortalMaintenanceJob.Utilities
{
    public class Segments
    {
        private ICreativeUnitService _CreativeUnitService;
        private IDocumentService _DocumentService;
        private ICampaignService _CampaignService;
        private IVideoTypeService _VideoTypeService;
        private IDocumentTypeService _DocumentTypeService;
        //private IVideoConversionCreativeUnitRepository _VideoConversionCreativeUnitRepository;
       // private static IAudienceListService _IntAudienceListService;
        private string DeviceIdFilesPath;
        //private double defaulVideoFPS = 30;
       // private int defaultAudioBitRate = 128;
        public Segments()
        {

            _CreativeUnitService = IoC.Instance.Resolve<ICreativeUnitService>();
            _DocumentService = IoC.Instance.Resolve<IDocumentService>();
            _CampaignService = IoC.Instance.Resolve<ICampaignService>();
            _VideoTypeService = IoC.Instance.Resolve<IVideoTypeService>();
            _DocumentTypeService = IoC.Instance.Resolve<IDocumentTypeService>();

          //  _IntAudienceListService = IoC.Instance.Resolve<IAudienceListService>();
            //_VideoConversionCreativeUnitRepository = IoC.Instance.Resolve<IVideoConversionCreativeUnitRepository>();
            //FilesPath = System.Configuration.ConfigurationManager.AppSettings["VideoFolderCreation"];
             DeviceIdFilesPath = System.Configuration.ConfigurationManager.AppSettings["DeviceIdFilesPath"];
          //  if (!string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["VideoAudioBitRate"]))
              //  defaultAudioBitRate = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["VideoAudioBitRate"]);
         //   if (!string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["VideoFPS"]))
             //   defaulVideoFPS = Convert.ToDouble(System.Configuration.ConfigurationManager.AppSettings["VideoFPS"]);

        }


        public void ResolveSegmentsFiles()
        {

            ApplicationContext.Instance.Logger.Info("Start Segments Files Process");
            char[] delimiterChars = { ' ', ',', '.', ':', '\t' };
            // get  Draft Video. 
            Server.Integration.Services.Client.IntegrationServiceClient clientServer = new Server.Integration.Services.Client.IntegrationServiceClient(Noqoush.AdFalcon.Domain.Configuration.WebAPIHostAdServer);
            try
                {
                    try
                    {
                       // string DeviceIdFilesPath = System.Configuration.ConfigurationManager.AppSettings["DeviceIdFilesPath"];






                        string[] filePaths = Directory.GetFiles(DeviceIdFilesPath);
                    string fileName = string.Empty;
                    string[] namesPart = null;
                    int AudienceList = 0;
                    int DevieType = 0;
                    foreach (string filePath in filePaths)
                        {

                         fileName = Path.GetFileName(filePath);
                        if (!fileName.Contains("Devices_Id_"))
                        {
                            continue;
                        }

                        fileName = fileName.Replace("Devices_Id_", "");
                        namesPart = fileName.Split('_');

                        int.TryParse(namesPart[0], out AudienceList);

                        int.TryParse(namesPart[1], out DevieType);
                        if (!(AudienceList > 0))
                        {
                            continue;
                        }
                        using (var f = new StreamReader(filePath))
                        {
                            string line = string.Empty;


                          
                           
                            while ((line = f.ReadLine()) != null)
                            {
                                var parts = line.Split(delimiterChars);

                                foreach (var item in parts)
                                {
                                    if (DevieType == 1)
                                    {
                                        Server.Integration.Services.Model.UpdateAudienceListRequest updateAudienceListRequest = new Server.Integration.Services.Model.UpdateAudienceListRequest();
                                        updateAudienceListRequest.AudienceSegmentId = AudienceList;
                                        updateAudienceListRequest.IDFA = item;
                                        updateAudienceListRequest.IDFASource = Server.Integration.Services.Model.IDFASource.Android;

                                        clientServer.UpdateAudienceList(updateAudienceListRequest);
                                    }
                                    else
                                    {
                                        Server.Integration.Services.Model.UpdateAudienceListRequest updateAudienceListRequest = new Server.Integration.Services.Model.UpdateAudienceListRequest();
                                        updateAudienceListRequest.AudienceSegmentId = AudienceList;
                                        updateAudienceListRequest.IDFA = item;
                                        updateAudienceListRequest.IDFASource = Server.Integration.Services.Model.IDFASource.IOS;

                                          clientServer.UpdateAudienceList(updateAudienceListRequest);
                                    }
                                }
                                //"Devices_Id_" + AudienceListId
                                //valuesCollection.Add(new Values(Convert.ToDateTime(parts[0]), Convert.ToInt32(parts[1]), parts[2]);
                            }
                        }

                        File.Delete(filePath);


                    }
                }
                    catch (Exception ex)
                    {
                        Framework.ApplicationContext.Instance.Logger.Error(ex.Message, ex);

                    }





                 


                        }
                        catch (Exception ex)
                        {
                            Framework.ApplicationContext.Instance.Logger.Error(ex.Message, ex);

                        }
                    
                  

                

            

            ApplicationContext.Instance.Logger.Info("Finish Segments Files Process");




        }

    }
}
