using Noqoush.AdFalcon.Domain.Common.Model.Core;
using Noqoush.AdFalcon.Domain.Common.Repositories.Campaign;
using Noqoush.AdFalcon.Domain.Common.Repositories.Campaign.Creative;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace Noqoush.AdFalcon.Domain.Common.Model.Campaign
{


    [DataContract()]
    public enum CreateOption
    {
        [EnumMember]
        Undefined = 0,
        [EnumMember]
        Upload = 1,
        [EnumMember]
        VAST = 2

    }



    [DataContract()]
    public enum VideoEndCardType
    {
        [EnumMember]
        Undefined = 0,
        [EnumMember]
        Static = 1,
        [EnumMember]
        Dynamic = 2

    }
    [DataContract()]
    public enum VASTProtocolsVersion
    {

        [EnumMember]
        None = 0,
        [EnumText("VAST2", "VideosTag")]
        [EnumMember]
        VAST2 = 1,
        [EnumText("VAST3", "VideosTag")]
        [EnumMember]
        VAST3 = 2,
        [EnumText("VAST4", "VideosTag")]
        [EnumMember]
        VAST4 = 3,
        [EnumText("VAST41", "VideosTag")]
        [EnumMember]
        VAST41 = 7,
               [EnumText("VAST42", "VideosTag")]
        [EnumMember]
        VAST42 = 9
    }


    [DataContract()]
    public enum Protocols
    {
        [EnumMember]
        None = 0,
        [EnumMember]
        VpaId1 = 1,
        [EnumMember]
        VpaId2 = 2
    }

    /*public class InStreamVideoCreative : AdCreative
    {
        private static IVideoMediaFileRepository _videoMediaFileRepository = null;
        private static IVideoMediaFileRepository VideoMediaFileRepository
        {
            get
            {
                if (_videoMediaFileRepository == null)
                {
                    _videoMediaFileRepository = Framework.IoC.Instance.Resolve<IVideoMediaFileRepository>();
                }
                return _videoMediaFileRepository;
            }
        }

        private const string CustomParamValue = "1";
        public InStreamVideoCreative()
        {
            TypeId = AdTypeIds.InStreamVideo;
            AdSubType = AdSubTypes.VideoLinear;
        }


        public virtual bool VideoEndCardFluid { get; set; }
        public virtual bool IsDraft { get; set; }
        public virtual bool IsXml
        {
            get
            {

                if (this.CreateOption == CreateOption.Upload)
                {
                    return false;
                }

                if (this.CreateOption == CreateOption.VAST)
                {
                    return true;
                }

                return false;
            }
            set { }
        }

        public virtual CreateOption CreateOption { get; set; }
        public virtual string Description { get; set; }

        public virtual int DurationInSeconds { get; set; }
        public virtual string ThirdPartyTag { get; set; }
        public override AdCreative Clone(AdGroup adGroup)
        {
            var cloneObj = base.Clone<InStreamVideoCreative>();

            cloneObj.DurationInSeconds = this.DurationInSeconds;
            cloneObj.Description = this.Description;
            cloneObj.IsDraft = this.IsDraft;

            cloneObj.ThirdPartyTag = this.ThirdPartyTag;
            cloneObj.CreateOption = this.CreateOption;
            cloneObj.VideoEndCardFluid = this.VideoEndCardFluid;
            cloneObj.Parent = this.Parent;

            foreach (var adCreativeUnit in AdCreativeUnits)
            {
                var adCreativeUnitClone = adCreativeUnit.Copy(cloneObj);
                if (adCreativeUnitClone.Document != null)
                {
                    adCreativeUnitClone.Document.UpdateUsage();
                }

                if (adCreativeUnitClone.SnapshotDocument != null)
                {
                    adCreativeUnitClone.SnapshotDocument.UpdateUsage();
                }

                cloneObj.AddCreativeUnit(adCreativeUnitClone);
            }

            if (VideoEndCards != null)
            {
                foreach (var videoEndCard in VideoEndCards)
                {
                    var videoEndCardClo = videoEndCard.Clone();


                    cloneObj.AddVideoEndCard((VideoEndCardCreative)videoEndCardClo);
                }
            }
            if (VideoEndCardTrackers != null)
            {
                foreach (var Tracker in VideoEndCardTrackers)
                {
                    var videoTrackerClone = Tracker.Clone();


                    cloneObj.AddVideoEndCardTracker(videoTrackerClone);
                }
            }


            return cloneObj;

        }

        public override AdCreative Clone()
        {
            return Clone(this.Group);
        }

        public override void Approve()
        {
            if (!this.IsDraft)
            {
                base.Approve();
                var ftpBaseDirectory = Configuration.FtpBaseDirectory;
                var cdnBaseUrl = Configuration.CdnBaseUrl;
                ftpUpload(ftpBaseDirectory, cdnBaseUrl);
                base.UploadSnapshots();
                if (VideoEndCards != null)
                {
                    foreach (var VideoEndCard in VideoEndCards)
                    {
                        VideoEndCard.Approve();
                    }
                }

                if (!string.IsNullOrWhiteSpace(this.GetWrapperContent()))
                {
                    this.ThirdPartyTag = this.GetWrapperContent();
                }
                else
                    this.ThirdPartyTag = null;

            }
            else
            {
                throw new BusinessException(new List<ErrorData> { new ErrorData("VideoDraftAdError") });
            }
        }

        public virtual IList<VideoEndCardCreative> VideoEndCards { get; set; }
        public virtual IList<VideoEndCardTracker> VideoEndCardTrackers { get; set; }
        public virtual void XmlUpload()
        {
            var ftpBaseDirectory = Configuration.FtpBaseDirectory;
            var cdnBaseUrl = Configuration.CdnBaseUrl;
            ftpUpload(ftpBaseDirectory, cdnBaseUrl);
        }

        public virtual void PublishVideoFiles()
        {
            var ftpBaseDirectory = Configuration.FtpBaseDirectory;
            var cdnBaseUrl = Configuration.CdnBaseUrl;
            if (this.AdCreativeUnits != null)
            {
                foreach (var adCreativeUnit in this.AdCreativeUnits)
                {

                    if (adCreativeUnit.MediaFiles != null)
                    {
                        foreach (var mediafile in adCreativeUnit.MediaFiles)
                        {


                            ftpUpload(ftpBaseDirectory, cdnBaseUrl, mediafile);
                        }


                    }
                }
            }

        }

        public virtual void ftpUpload(string baseDirectory, string cdnBaseUrl, VideoMediaFile mediaFile)
        {

            string temp = string.Empty;
            var subFolder = string.Empty;
            if (!string.IsNullOrWhiteSpace(Group.Campaign.FolderURL))
            {
                subFolder = Group.Campaign.FolderURL;
                ApplicationContext.Instance.Logger.Info(string.Format("Campaign Folder Already Exists - AdId: {0}, CampaginID: {1}, Campaign Folder: {2}", this.ID, Group.Campaign.ID, Group.Campaign.FolderURL));
            }
            else
            {
                ApplicationContext.Instance.Logger.Info(string.Format("Campaign Folder Not Exist - AdId: {0}, CampaginID: {1}", this.ID, Group.Campaign.ID));

                //we need to create folder for it
                subFolder = Framework.Utilities.Environment.GetServerTime().ToString("yyyyMMdd");
                temp = string.Format("{0}/{1}", baseDirectory, subFolder);
                //create folder fo the current date
                Framework.Utilities.FtpHelper.CreateDirectory(temp);
                //create folder for current Campaign
                var isFolderCreated = false;
                while (!isFolderCreated)
                {
                    temp = string.Format("{0}/{1}", baseDirectory, subFolder);
                    var r = RandomNumber(1, 100000);
                    temp = string.Format("{0}/{1}", temp, r);
                    if (!Framework.Utilities.FtpHelper.DirectoryExists(temp))
                    {
                        Framework.Utilities.FtpHelper.CreateDirectory(temp);
                        isFolderCreated = true;
                        subFolder += "/" + r;

                        ApplicationContext.Instance.Logger.Info(string.Format("Campaign Folder Created - AdId: {0}, CampaginID: {1}, Campaign Folder: {2}", this.ID, Group.Campaign.ID, subFolder));
                        Group.Campaign.FolderURL = subFolder;
                    }
                }
            }

            // create folder for current ad if not found

            if (!Framework.Utilities.FtpHelper.DirectoryExists((baseDirectory + "/"+subFolder))  )
            {
                ApplicationContext.Instance.Logger.Info(string.Format("SubFolder:{0}", (baseDirectory + "/" + subFolder)));
                  Framework.Utilities.FtpHelper.CreateDirectory((baseDirectory + "/" + subFolder));
            }
            subFolder = string.Format("{0}/{1}", subFolder, uId);

           
            temp = string.Format("{0}/{1}", baseDirectory, subFolder);
            if (!Framework.Utilities.FtpHelper.DirectoryExists(temp))
            {
                Framework.Utilities.FtpHelper.CreateDirectory(temp);
            }
            var directory = string.Format("{0}/{1}", baseDirectory, subFolder);
            var cdnUrl = string.Format("{0}/{1}", cdnBaseUrl, subFolder);

            //var duplicates = AdCreativeUnits.Where(M => M.Document != null).GroupBy(s => s.Document.Name.ToLower()).SelectMany(grp => grp.Skip(1));

            if (mediaFile.Document != null)
                mediaFile.Document.NewName = string.Format("{0}_{1}_{2}", mediaFile.Document.GetNameWithNoExtension(),
                                                            mediaFile.OriginalCreativeUnit.Width,
                                                              mediaFile.OriginalCreativeUnit.Height);

            //foreach (var adCreativeUnit in AdCreativeUnits)
            {

                if (mediaFile.Document != null)
                {
                    string url = mediaFile.Document.ftpUpload(directory, cdnUrl);
                    mediaFile.URL = url;

                    // VideoMediaFileRepository.Save(mediaFile);



                }
            }

        }
        public virtual void ftpUpload(string baseDirectory, string cdnBaseUrl)
        {
            string temp = string.Empty;
            var subFolder = string.Empty;
            if (!string.IsNullOrWhiteSpace(Group.Campaign.FolderURL))
            {
                subFolder = Group.Campaign.FolderURL;
                ApplicationContext.Instance.Logger.Info(string.Format("Campaign Folder Already Exists - AdId: {0}, CampaginID: {1}, Campaign Folder: {2}", this.ID, Group.Campaign.ID, Group.Campaign.FolderURL));
            }
            else
            {
                ApplicationContext.Instance.Logger.Info(string.Format("Campaign Folder Not Exist - AdId: {0}, CampaginID: {1}", this.ID, Group.Campaign.ID));

                //we need to create folder for it
                subFolder = Framework.Utilities.Environment.GetServerTime().ToString("yyyyMMdd");
                temp = string.Format("{0}/{1}", baseDirectory, subFolder);
                //create folder fo the current date
                Framework.Utilities.FtpHelper.CreateDirectory(temp);
                //create folder for current Campaign
                var isFolderCreated = false;
                while (!isFolderCreated)
                {
                    temp = string.Format("{0}/{1}", baseDirectory, subFolder);
                    var r = RandomNumber(1, 100000);
                    temp = string.Format("{0}/{1}", temp, r);
                    if (!Framework.Utilities.FtpHelper.DirectoryExists(temp))
                    {
                        Framework.Utilities.FtpHelper.CreateDirectory(temp);
                        isFolderCreated = true;
                        subFolder += "/" + r;

                        ApplicationContext.Instance.Logger.Info(string.Format("Campaign Folder Created - AdId: {0}, CampaginID: {1}, Campaign Folder: {2}", this.ID, Group.Campaign.ID, subFolder));
                        Group.Campaign.FolderURL = subFolder;
                    }
                }
            }

            // create folder for current ad if not found
            subFolder = string.Format("{0}/{1}", subFolder, uId);
            temp = string.Format("{0}/{1}", baseDirectory, subFolder);
            Framework.Utilities.FtpHelper.CreateDirectory(temp);

            var directory = string.Format("{0}/{1}", baseDirectory, subFolder);
            var cdnUrl = string.Format("{0}/{1}", cdnBaseUrl, subFolder);

            var duplicates = AdCreativeUnits.Where(M => M.Document != null).GroupBy(s => s.Document.Name.ToLower()).SelectMany(grp => grp.Skip(1));

            foreach (var adCreativeUnit in duplicates)
            {
                adCreativeUnit.Document.NewName = string.Format("{0}_{1}_{2}", adCreativeUnit.Document.GetNameWithNoExtension(),
                                                             adCreativeUnit.CreativeUnit.Width,
                                                             adCreativeUnit.CreativeUnit.Height);
            }
            foreach (var adCreativeUnit in AdCreativeUnits)
            {

                if (adCreativeUnit.Document != null)
                {if (this.CreateOption == CreateOption.Upload)
                    {
                        try
                        {
                            //For THe Release
                            string url = adCreativeUnit.Document.ftpUpload(directory, cdnUrl);
                            adCreativeUnit.Content = url;
                        }
                        catch
                        { 
                        
                        }
                    }
                    else
                    {
                        string url = adCreativeUnit.Document.ftpUpload(directory, cdnUrl);
                        adCreativeUnit.Content = url;

                    }



                    // if (!adCreativeUnit.KeepShapshot)
                    {
                        adCreativeUnit.SnapshotUrl = adCreativeUnit.Content;
                        adCreativeUnit.KeepShapshot = true;
                    }


                }
            }
        }


        //public virtual IList<VideoMediaFile> MediaFiles { get; set; }


        //public virtual void AddMediaFile(VideoMediaFile mediaFile)
        //{

        //    if (MediaFiles==null)
        //    {
        //        MediaFiles = new List<VideoMediaFile>();
        //    }

        //    MediaFiles.Add(mediaFile);
        //}

        //public virtual void RemoveMediaFiles()
        //{
        //    if (MediaFiles != null)
        //    {
        //        MediaFiles.Clear();
        //    }



        //}
        public virtual void AddVideoEndCard(VideoEndCardCreative videoEndCard)
        {

            if (VideoEndCards == null)
            {
                VideoEndCards = new List<VideoEndCardCreative>();
            }

            VideoEndCards.Add(videoEndCard);
        }


        //public virtual void SetAllCreativeUnitUnused()
        //{
        //    foreach (var adCreativeUnit in AdCreativeUnits)
        //    {
        //        adCreativeUnit.Document.UpdateUsage(isRemove: true);
        //    }
        //}

        public virtual void AddVideoEndCardTracker(VideoEndCardTracker videoEndCardTracker)
        {

            if (VideoEndCardTrackers == null)
            {
                VideoEndCardTrackers = new List<VideoEndCardTracker>();
            }

            VideoEndCardTrackers.Add(videoEndCardTracker);
        }

        public virtual void SetProtocol(Protocols Protocol, bool IsRequired)
        {
            string name = Enum.Parse(typeof(Protocols), Protocol.ToString()).ToString().ToLower();
            AdCustomParameter adCustomParam = GetCustomParameter();
            if (adCustomParam != null)
            {
                if (Protocol == Protocols.None)
                {
                    RemoveAdCustomParameter(adCustomParam);
                }
                else
                {
                    adCustomParam.IsMandatory = IsRequired;
                    adCustomParam.Name = name;
                    adCustomParam.IsDeleted = false;
                }
            }
            else
            {
                if (Protocol != Protocols.None)
                {
                    AddAdCustomParameter(name, CustomParamValue, IsRequired);
                }
            }
        }
        public virtual AdCustomParameter GetCustomParameter()
        {
            string VpId1 = Protocols.VpaId1.ToString().ToLower();
            string VpId2 = Protocols.VpaId2.ToString().ToLower();
            return this.AdCustomParameters.Where(x => x.Name == VpId1 || x.Name == VpId2).FirstOrDefault();
        }

        //public InStreamVideoCreativeUnit GetInstreamVideoCreativeUnit()
        //{
        //    AdCreativeUnit adCreativeUnit = GetCreativeUnits().FirstOrDefault();
        //    if (adCreativeUnit != null)
        //    {
        //        return IoC.Instance.Resolve<IInStreamVideoCreativeUnitRepository>().Get(adCreativeUnit.ID);
        //    }
        //    return null;
        //}
    }


    public class VideoEndCardCreative : AdCreative
    {
        public VideoEndCardCreative()
        {
            TypeId = AdTypeIds.VideoEndCard;
            AdSubType = AdSubTypes.VideoEndCard;
        }
        public virtual bool Fluid { get; set; }
        public virtual VideoEndCardType CardType { get; set; }
        public virtual double? AutoCloseWaitInSeconds { get; set; }
        public virtual bool EnableAutoClose { get; set; }
        public virtual void ClearCreativeUnits()
        {
            // var unusedItems = AdCreativeUnits.Where(item => item.Document.Usage < 1).ToList();
            foreach (var unusedItem in AdCreativeUnits)
            {
                //AdCreativeUnits.Remove(unusedItem);
                unusedItem.Delete();
            }
        }

        public override void Approve()
        {

            base.Approve();
            var ftpBaseDirectory = Configuration.FtpBaseDirectory;
            var cdnBaseUrl = Configuration.CdnBaseUrl;
            ftpUpload(ftpBaseDirectory, cdnBaseUrl);


        }
        public override AdCreative Clone(AdGroup adGroup)
        {
            var cloneObj = base.Clone<VideoEndCardCreative>();
            cloneObj.EnableAutoClose = this.EnableAutoClose;
            cloneObj.AutoCloseWaitInSeconds = this.AutoCloseWaitInSeconds;
            cloneObj.Fluid = this.Fluid;
            cloneObj.Parent = this.Parent;
            cloneObj.CardType = this.CardType;


     
            if (this.ThirdPartyTrackers != null && this.ThirdPartyTrackers.Count > 0)
            {
                cloneObj.ThirdPartyTrackers = new List<ThirdPartyTracker>();
                foreach (var thirdParty in this.ThirdPartyTrackers)
                {
                    cloneObj.ThirdPartyTrackers.Add(thirdParty.Clone());
                }
            }

            foreach (var adCreativeUnit in AdCreativeUnits)
            {
                var adCreativeUnitClone = adCreativeUnit.Copy(cloneObj);
                if (adCreativeUnitClone.Document != null)
                {
                    adCreativeUnitClone.Document.UpdateUsage();
                }

                if (adCreativeUnitClone.SnapshotDocument != null)
                {
                    adCreativeUnitClone.SnapshotDocument.UpdateUsage();
                }

                cloneObj.AddCreativeUnit(adCreativeUnitClone);
            }

            return cloneObj;

        }

        public override AdCreative Clone()
        {
            return Clone(this.Group);
        }
        public virtual void ftpUpload(string baseDirectory, string cdnBaseUrl)
        {
            string temp = string.Empty;
            var subFolder = string.Empty;
            if (!string.IsNullOrWhiteSpace(Group.Campaign.FolderURL))
            {
                subFolder = Group.Campaign.FolderURL;
                ApplicationContext.Instance.Logger.Info(string.Format("Campaign Folder Already Exists - AdId: {0}, CampaginID: {1}, Campaign Folder: {2}", this.ID, Group.Campaign.ID, Group.Campaign.FolderURL));
            }
            else
            {
                ApplicationContext.Instance.Logger.Info(string.Format("Campaign Folder Not Exist - AdId: {0}, CampaginID: {1}", this.ID, Group.Campaign.ID));

                //we need to create folder for it
                subFolder = Framework.Utilities.Environment.GetServerTime().ToString("yyyyMMdd");
                temp = string.Format("{0}/{1}", baseDirectory, subFolder);
                //create folder fo the current date
                Framework.Utilities.FtpHelper.CreateDirectory(temp);
                //create folder for current Campaign
                var isFolderCreated = false;
                while (!isFolderCreated)
                {
                    temp = string.Format("{0}/{1}", baseDirectory, subFolder);
                    var r = RandomNumber(1, 100000);
                    temp = string.Format("{0}/{1}", temp, r);
                    if (!Framework.Utilities.FtpHelper.DirectoryExists(temp))
                    {
                        Framework.Utilities.FtpHelper.CreateDirectory(temp);
                        isFolderCreated = true;
                        subFolder += "/" + r;

                        ApplicationContext.Instance.Logger.Info(string.Format("Campaign Folder Created - AdId: {0}, CampaginID: {1}, Campaign Folder: {2}", this.ID, Group.Campaign.ID, subFolder));
                        Group.Campaign.FolderURL = subFolder;
                    }
                }
            }

            // create folder for current ad if not found
            subFolder = string.Format("{0}/{1}", subFolder, uId);
            temp = string.Format("{0}/{1}", baseDirectory, subFolder);
            Framework.Utilities.FtpHelper.CreateDirectory(temp);

            var directory = string.Format("{0}/{1}", baseDirectory, subFolder);
            var cdnUrl = string.Format("{0}/{1}", cdnBaseUrl, subFolder);

            var duplicates = AdCreativeUnits.Where(M => M.Document != null).GroupBy(s => s.Document.Name.ToLower()).SelectMany(grp => grp.Skip(1));
            foreach (var adCreativeUnit in duplicates)
            {
                adCreativeUnit.Document.NewName = string.Format("{0}_{1}_{2}", adCreativeUnit.Document.GetNameWithNoExtension(),
                                                             adCreativeUnit.CreativeUnit.Width,
                                                             adCreativeUnit.CreativeUnit.Height);
            }
            foreach (var adCreativeUnit in AdCreativeUnits)
            {
                if (adCreativeUnit.Document != null)
                {
                    if (string.IsNullOrWhiteSpace(adCreativeUnit.Content))
                    {
                        string url = adCreativeUnit.Document.ftpUpload(directory, cdnUrl);
                        adCreativeUnit.Content = url;
                    }

                    //if (!adCreativeUnit.KeepShapshot)
                    {
                        adCreativeUnit.SnapshotUrl = adCreativeUnit.Content;
                        adCreativeUnit.KeepShapshot = true;
                    }
                }
                adCreativeUnit.ImageType = adCreativeUnit.Document != null ? adCreativeUnit.Document.Extension.Trim('.') : null;



            }
        }
    }*/
}