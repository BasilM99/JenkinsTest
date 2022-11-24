using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Noqoush.AdFalcon.Domain.Common.Model.Campaign;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.AppSite;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Campaign.Creative;
using Noqoush.AdFalcon.Web.Controllers.Model.Campaign;

namespace Noqoush.AdFalcon.Web.Controllers.Model.Core
{


    public class UploadViewModel
    {
        public UploadViewModel()
        {
            EnvironmentType = EnvironmentType.All;
            VideoInformation = new VideoInformationModel();
        }
        public bool IsAllowedToSaveImpressionTracker { get; set; }
        public int? DocId { get; set; }
        public int? ThubnailDocId { get; set; }
        public int? TileActionDocID { get; set; }
        public string TypeId { get; set; }
        public string ParentId { get; set; }
        public string Name { get; set; }
        public string DisplayText { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int PreviewWidth { get; set; }
        public int PreviewHeight { get; set; }
        public bool ShowCopy { get; set; }
        public int AdTypeId { get; set; }
        public int AdSubTypeId { get; set; }
        public IList<FormatDto> Formats { get; set; }
        public int RequiredType { get; set; }
        public EnvironmentType EnvironmentType { get; set; }
        public OrientationType OrientationType { get; set; }
        public string ImpressionTrackerRedirect { get; set; }
                public string ImpressionTrackerJSRedirect { get; set; }

        
        public static UploadViewModel ParseCreativeUnit(CreativeUnitViewModel creativeUnitView)
        {
            UploadViewModel uploadViewModel = new UploadViewModel
                {
                    DocId = creativeUnitView.DocumentId,
                    ParentId = creativeUnitView.CreativeUnitDto.ID.ToString(),
                    TypeId = ((int)creativeUnitView.DeviceType).ToString(),
                    DisplayText = creativeUnitView.DisplayText,
                    Width = creativeUnitView.CreativeUnitDto.Width,
                    PreviewWidth = creativeUnitView.CreativeUnitDto.PreviewWidth,
                    Name = creativeUnitView.Name,
                    Height = creativeUnitView.CreativeUnitDto.Height,
                    PreviewHeight = creativeUnitView.CreativeUnitDto.PreviewHeight,
                    Formats = creativeUnitView.CreativeUnitDto.Formats,
                    RequiredType = creativeUnitView.CreativeUnitDto.RequiredType,
                    EnvironmentType = creativeUnitView.CreativeUnitDto.EnvironmentType,
                    OrientationType = creativeUnitView.CreativeUnitDto.OrientationType,
                    ShowCopy = creativeUnitView.ShowCopy,
                    AdTypeId = creativeUnitView.AdTypeId,
                    AdSubTypeId= creativeUnitView.AdSubTypeId,
                    ImpressionTrackerRedirect = creativeUnitView.ImpressionTrackerRedirect,
                ImpressionTrackerJSRedirect = creativeUnitView.ImpressionTrackerJSRedirect,
                IsAllowedToSaveImpressionTracker = creativeUnitView.IsAllowedToSaveImpressionTracker,
              

    };
            if (creativeUnitView.InStreamVideoCreativeUnit != null)
            {
                uploadViewModel.ThubnailDocId = creativeUnitView.InStreamVideoCreativeUnit.ThumbnailDocId;
                uploadViewModel.VideoInformation = new VideoInformationModel()
                {
                    FormatName=creativeUnitView.InStreamVideoCreativeUnit.VideoTypeCode,
                    BitRate = creativeUnitView.InStreamVideoCreativeUnit.BitRate,
                    Duration = creativeUnitView.InStreamVideoCreativeUnit.VideoDuration,
                    Height = creativeUnitView.InStreamVideoCreativeUnit.VideoHeight,
                    Width = creativeUnitView.InStreamVideoCreativeUnit.VideoWidth
                };
            }
            return uploadViewModel;
        }
        public static UploadViewModel ParseTileImage(CreativeUnitViewModel creativeUnitView)
        {
            return new UploadViewModel
                {
                    DocId = creativeUnitView.DocumentId,
                    ParentId = creativeUnitView.TileImageDocumentDto.TileImageSize.TitleSize.ID.ToString(),
                    TypeId = "",
                    DisplayText = creativeUnitView.DisplayText,
                    Width = creativeUnitView.TileImageDocumentDto.TileImageSize.Width,
                    Name = creativeUnitView.Name,
                    Height = creativeUnitView.TileImageDocumentDto.TileImageSize.Height,
                    Formats = creativeUnitView.TileImageDocumentDto.TileImageSize.Formats,
                    EnvironmentType = EnvironmentType.All,
                    OrientationType = OrientationType.Any,
                    TileActionDocID = creativeUnitView.TileImageDocumentDto.Document.ID,
                    ImpressionTrackerRedirect = creativeUnitView.ImpressionTrackerRedirect,
                ImpressionTrackerJSRedirect = creativeUnitView.ImpressionTrackerJSRedirect,
                IsAllowedToSaveImpressionTracker = creativeUnitView.IsAllowedToSaveImpressionTracker,

            };
        }
        public static UploadViewModel ParseImage(CreativeUnitViewModel creativeUnitView)
        {
            return new UploadViewModel
            {
                DocId = creativeUnitView.DocumentId,
                ParentId ="",
                TypeId = "",
                DisplayText = creativeUnitView.DisplayText,
                Width = 80,
                Name = creativeUnitView.Name,
                Height = 70,
                Formats = creativeUnitView.TileImageDocumentDto.TileImageSize.Formats,
                EnvironmentType = EnvironmentType.All,
                OrientationType = OrientationType.Any,
                TileActionDocID = null,
                PreviewWidth = 80,
             
                PreviewHeight = 70
            };
        }

        public VideoInformationModel VideoInformation { get; set; }
    }

}
